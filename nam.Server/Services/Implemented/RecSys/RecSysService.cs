using System.Text.Json;
using DataInjection.Qdrant.Serializers;
using Domain.Entities;
using Microsoft.Extensions.AI;
using nam.Server.Services.Interfaces;
using nam.Server.Services.Interfaces.RecSys;
using Qdrant.Client;
using Qdrant.Client.Grpc;

namespace nam.Server.Services.Implemented.RecSys
{
    /// <summary>
    /// Recommendation service that:
    /// - retrieves the user questionnaire,
    /// - generates an embedding from all embeddable questionnaire fields,
    /// - searches a Qdrant vector collection of POIs at chunk level,
    /// - groups results by POI id,
    /// - scores POIs based on vector similarity, distance, and category match,
    /// - and returns ranked POI identifiers.
    /// </summary>
    public class RecsysService : IRecSysService
    {
        private readonly IUserService _userService;
        private readonly QdrantClient _qdrantClient;
        private readonly IEmbeddingGenerator<string, Embedding<float>> _generator;
        private readonly IScorer _scorer;
        private readonly IRanker _ranker;

        /// <summary>
        /// When true, the service ignores the real location and uses the test coordinates.
        /// Useful for local testing and debugging.
        /// </summary>
        private readonly bool _forceTestLocation = true;

        /// <summary>
        /// Test latitude used when <see cref="_forceTestLocation"/> is true.
        /// </summary>
        private readonly double _testLat = 43.255; // Matelica

        /// <summary>
        /// Test longitude used when <see cref="_forceTestLocation"/> is true.
        /// </summary>
        private readonly double _testLon = 13.0115;

        private const string CollectionName = "POI-vectors";
        private const int DefaultLimit = 15;

        public RecsysService(
            IUserService userService,
            QdrantClient qdrantClient,
            IEmbeddingGenerator<string, Embedding<float>> generator,
            IScorer scorer,
            IRanker ranker)
        {
            _userService = userService;
            _qdrantClient = qdrantClient;
            _generator = generator;
            _scorer = scorer;
            _ranker = ranker;
        }

        /// <summary>
        /// Generates POI recommendations for the given user.
        /// </summary>
        /// <param name="userEmail">Email used to identify the user and load their questionnaire.</param>
        /// <param name="realLat">Current user latitude, if available.</param>
        /// <param name="realLon">Current user longitude, if available.</param>
        /// <returns>List of recommended POI identifiers, ordered by descending relevance.</returns>
        public async Task<List<Guid>> GetRecommendationsAsync(string userEmail, double? realLat, double? realLon)
        {
            // 1. Determine user location (optionally forced to test coordinates).
            var lat = _forceTestLocation ? _testLat : realLat;
            var lon = _forceTestLocation ? _testLon : realLon;
            var hasLocation = lat.HasValue && lon.HasValue && lat.Value != 0 && lon.Value != 0;

            // 2. Load questionnaire. If none exists, return generic fallback.
            var questionnaire = await _userService.GetQuestionaireByUserMailAsync(userEmail);
            if (questionnaire is null)
            {
                return await GetGenericFallbackAsync(DefaultLimit, new HashSet<Guid>());
            }

            // 3. Generate an embedding from all [Embeddable] questionnaire fields.
            var textToEmbed = questionnaire.ToEmbeddingString();
            var embeddingResult = await _generator.GenerateAsync(textToEmbed);

            if (embeddingResult == null)
            {
                return await GetGenericFallbackAsync(DefaultLimit, new HashSet<Guid>());
            }

            var userVector = embeddingResult.Vector;

            // Categories selected in the questionnaire, used for category matching.
            var preferredCategories = questionnaire.Interest ?? new List<string>();

            // 4. Multi-stage recommendation pipeline.
            var scoredPois = new List<ScoredPoi>();
            var excludedPoiIds = new HashSet<Guid>();
            var targetCount = DefaultLimit;

            // Stage 1: vector + tight radius (10 km)
            if (hasLocation)
            {
                var stage1Chunks = await SearchPoiChunksAsync(
                    vector: userVector,
                    lat: lat,
                    lon: lon,
                    radiusKm: 10,
                    excludedPoiIds: excludedPoiIds,
                    limit: targetCount * 3);

                GroupScoreAndAdd(scoredPois, excludedPoiIds, stage1Chunks, lat, lon, preferredCategories);
            }

            if (scoredPois.Count >= targetCount)
                return RankAndSelect(scoredPois, targetCount);

            // Stage 2: vector only, no radius
            var stage2Chunks = await SearchPoiChunksAsync(
                vector: userVector,
                lat: null,
                lon: null,
                radiusKm: null,
                excludedPoiIds: excludedPoiIds,
                limit: targetCount * 4);

            GroupScoreAndAdd(scoredPois, excludedPoiIds, stage2Chunks, lat, lon, preferredCategories);

            if (scoredPois.Count >= targetCount)
                return RankAndSelect(scoredPois, targetCount);

            // Stage 3: vector + wider radius (50 km)
            if (hasLocation)
            {
                var stage3Chunks = await SearchPoiChunksAsync(
                    vector: userVector,
                    lat: lat,
                    lon: lon,
                    radiusKm: 50,
                    excludedPoiIds: excludedPoiIds,
                    limit: targetCount * 4);

                GroupScoreAndAdd(scoredPois, excludedPoiIds, stage3Chunks, lat, lon, preferredCategories);
            }

            if (scoredPois.Count >= targetCount)
                return RankAndSelect(scoredPois, targetCount);

            // Stage 4: fallback using scroll (no embedding).
            var needed = targetCount - scoredPois.Count;
            var fallbackIds = await GetGenericFallbackAsync(needed, excludedPoiIds);

            var rankedIds = RankAndSelect(scoredPois, targetCount);
            rankedIds.AddRange(fallbackIds);

            return rankedIds
                .Distinct()
                .Take(targetCount)
                .ToList();
        }

        #region Qdrant Search

        /// <summary>
        /// Performs a vector search on Qdrant at chunk level, optionally excluding already-selected POI ids.
        /// Geo filtering is done at application level using Haversine distance.
        /// </summary>
        private async Task<IReadOnlyList<ScoredPoint>> SearchPoiChunksAsync(
            ReadOnlyMemory<float> vector,
            double? lat,
            double? lon,
            double? radiusKm,
            HashSet<Guid> excludedPoiIds,
            int limit)
        {
            var filter = new Filter();

            // Exclude POIs that have already been selected in previous stages via a MustNot / HasId condition.
            if (excludedPoiIds.Count > 0)
            {
                var pointIds = excludedPoiIds
                    .Select(id => new PointId { Uuid = id.ToString() })
                    .ToArray();

                filter.MustNot.Add(new Condition
                {
                    HasId = new HasIdCondition { HasId = { pointIds } }
                });
            }

            // Note: radiusKm / lat / lon are not used inside Qdrant here.
            // Spatial scoring is handled via Haversine at application level.
            // If you add a GeoPoint payload later, you can add a geo filter here.

            var finalFilter = (filter.Must.Count > 0 || filter.MustNot.Count > 0)
                ? filter
                : null;

            var result = await _qdrantClient.SearchAsync(
                collectionName: CollectionName,
                vector: vector,
                filter: finalFilter,
                limit: (ulong)limit
            );

            return result;
        }

        #endregion

        #region Grouping & Scoring

        /// <summary>
        /// Groups chunk-level results by POI id, computes a final score per POI,
        /// and adds them to the target list, updating the exclusion set.
        /// </summary>
        private void GroupScoreAndAdd(
            List<ScoredPoi> targetList,
            HashSet<Guid> excludedPoiIds,
            IReadOnlyList<ScoredPoint> chunkPoints,
            double? userLat,
            double? userLon,
            List<string> preferredCategories)
        {
            var groupedByPoi = chunkPoints
                .Where(p => p.Id?.Uuid is not null && Guid.TryParse(p.Id.Uuid, out _))
                .GroupBy(p => Guid.Parse(p.Id.Uuid));

            foreach (var group in groupedByPoi)
            {
                var poiId = group.Key;
                if (excludedPoiIds.Contains(poiId))
                    continue;

                // Choose the chunk with the highest vector score as representative.
                var bestChunk = group
                    .OrderByDescending(p => p.Score)
                    .First();

                // Extract POI location from payload and compute distance.
                ExtractPoiGeo(bestChunk, out var poiLat, out var poiLon);

                double? distanceKm = null;
                if (userLat.HasValue && userLon.HasValue && poiLat.HasValue && poiLon.HasValue)
                {
                    distanceKm = HaversineDistanceKm(
                        userLat.Value, userLon.Value,
                        poiLat.Value, poiLon.Value);
                }

                // Extract POI category from payload and compute final score.
                var poiCategory = ExtractPoiCategory(bestChunk);

                var finalScore = _scorer.CalculateScore(
                    vectorScore: bestChunk.Score,
                    distanceKm: distanceKm,
                    itemCategory: poiCategory,
                    preferredCategories: preferredCategories
                );

                targetList.Add(new ScoredPoi
                {
                    PoiId = poiId,
                    FinalScore = finalScore
                });

                excludedPoiIds.Add(poiId);
            }
        }

        /// <summary>
        /// Extracts the POI category from the scored point payload.
        /// Expects a payload key named "category".
        /// </summary>
        private static string? ExtractPoiCategory(ScoredPoint point)
        {
            if (point.Payload is null)
                return null;

            if (!point.Payload.TryGetValue("category", out var catVal))
                return null;

            var raw = catVal?.ToString();
            return string.IsNullOrWhiteSpace(raw)
                ? null
                : raw.Trim('"');
        }

        /// <summary>
        /// Extracts POI latitude and longitude from the scored point payload.
        /// Expects payload keys "lat" and "lon".
        /// </summary>
        private static void ExtractPoiGeo(ScoredPoint point, out double? lat, out double? lon)
        {
            lat = null;
            lon = null;

            if (point.Payload is null)
                return;

            if (point.Payload.TryGetValue("lat", out var latVal) &&
                TryParseDouble(latVal, out var dLat))
            {
                lat = dLat;
            }

            if (point.Payload.TryGetValue("lon", out var lonVal) &&
                TryParseDouble(lonVal, out var dLon))
            {
                lon = dLon;
            }
        }

        /// <summary>
        /// Attempts to parse a Qdrant gRPC value or a standard .NET value into a double.
        /// </summary>
        private static bool TryParseDouble(object? value, out double result)
        {
            result = 0;
            if (value is null) return false;

            // Handle Qdrant gRPC Value type.
            if (value is Qdrant.Client.Grpc.Value qValue)
            {
                if (qValue.KindCase == Qdrant.Client.Grpc.Value.KindOneofCase.DoubleValue)
                {
                    result = qValue.DoubleValue;
                    return true;
                }

                if (qValue.KindCase == Qdrant.Client.Grpc.Value.KindOneofCase.IntegerValue)
                {
                    result = qValue.IntegerValue;
                    return true;
                }

                return double.TryParse(
                    qValue.ToString(),
                    System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture,
                    out result);
            }

            // Fallback: use ToString() for standard .NET types.
            return double.TryParse(
                value.ToString(),
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out result);
        }

        /// <summary>
        /// Computes the Haversine distance between two coordinates, in kilometers.
        /// </summary>
        private static double HaversineDistanceKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth radius in km
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            var a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) *
                Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double DegreesToRadians(double deg) => deg * (Math.PI / 180.0);

        #endregion

        #region Ranking & Fallback

        /// <summary>
        /// Uses the configured ranker to order POIs by final score
        /// and return the top N identifiers.
        /// </summary>
        private List<Guid> RankAndSelect(List<ScoredPoi> scoredPois, int take)
        {
            var tuples = scoredPois.Select(p => (p.PoiId, p.FinalScore));
            return _ranker.RankAndSelect(tuples, take);
        }

        /// <summary>
        /// Generic fallback that scrolls Qdrant for arbitrary POIs (chunk-level),
        /// groups them by POI id, and returns distinct ids not already excluded.
        /// </summary>
        private async Task<List<Guid>> GetGenericFallbackAsync(int count, HashSet<Guid> excludedPoiIds)
        {
            if (count <= 0)
                return new List<Guid>();

            var filter = new Filter();

            if (excludedPoiIds.Count > 0)
            {
                var pointIds = excludedPoiIds
                    .Select(id => new PointId { Uuid = id.ToString() })
                    .ToArray();

                filter.MustNot.Add(new Condition
                {
                    HasId = new HasIdCondition { HasId = { pointIds } }
                });
            }

            var finalFilter = (filter.Must.Count > 0 || filter.MustNot.Count > 0)
                ? filter
                : null;

            var scrollResult = await _qdrantClient.ScrollAsync(
                collectionName: CollectionName,
                filter: finalFilter,
                limit: (uint)(count * 3)
            );

            return scrollResult.Result
                .Where(p => p.Id?.Uuid is not null && Guid.TryParse(p.Id.Uuid, out _))
                .Select(p => Guid.Parse(p.Id.Uuid))
                .Where(id => !excludedPoiIds.Contains(id))
                .Distinct()
                .Take(count)
                .ToList();
        }

        #endregion

        /// <summary>
        /// Internal representation of a scored POI before final ranking.
        /// </summary>
        private class ScoredPoi
        {
            public Guid PoiId { get; set; }
            public double FinalScore { get; set; }
        }
    }
}