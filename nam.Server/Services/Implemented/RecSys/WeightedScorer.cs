using nam.Server.Services.Interfaces.RecSys;

namespace nam.Server.Services.Implemented.RecSys
{
    /// <summary>
    /// Combines vector similarity, geographic distance, and category match
    /// into a single relevance score for a POI.
    /// </summary>
    public class WeightedScorer : IScorer
    {
        /// <summary>
        /// Weight of the embedding similarity component.
        /// </summary>
        private const double WeightVector = 0.60;

        /// <summary>
        /// Weight of the geographic distance component.
        /// </summary>
        private const double WeightDistance = 0.15;

        /// <summary>
        /// Weight of the category match component.
        /// </summary>
        private const double WeightCategory = 0.25;

        /// <summary>
        /// Maximum distance in kilometers over which distance has an influence.
        /// Beyond this, the distance contribution is effectively zero.
        /// </summary>
        private const double MaxDistanceInfluenceKm = 30.0;

        /// <summary>
        /// Calculates a final score given:
        /// - a vector similarity score,
        /// - an optional distance in km between user and POI,
        /// - a POI category and a list of preferred user categories.
        /// </summary>
        /// <param name="vectorScore">
        /// Similarity score in [0,1] (or at least normalized comparably).
        /// </param>
        /// <param name="distanceKm">
        /// Distance in kilometers between the user and the POI, if known.
        /// </param>
        /// <param name="itemCategory">
        /// Category of the POI (e.g. "museum", "restaurant").
        /// </param>
        /// <param name="preferredCategories">
        /// Categories selected in the user questionnaire.
        /// </param>
        /// <returns>A combined score, higher is better.</returns>
        public double CalculateScore(
            double vectorScore,
            double? distanceKm,
            string? itemCategory,
            List<string>? preferredCategories)
        {
            // 1. Vector (embedding) score, clamped to [0,1] for safety.
            var vScore = Clamp01(vectorScore);

            // 2. Distance contribution: linear decay from 1 at 0km to 0 at MaxDistanceInfluenceKm+.
            double dScore = 0.0;
            if (distanceKm.HasValue)
            {
                var raw = 1.0 - (distanceKm.Value / MaxDistanceInfluenceKm);
                dScore = Clamp01(raw);
            }

            // 3. Category contribution: 1 if POI category is in the preferred categories, 0 otherwise.
            double cScore = 0.0;
            if (preferredCategories is { Count: > 0 } &&
                !string.IsNullOrWhiteSpace(itemCategory) &&
                preferredCategories.Any(c =>
                    c.Equals(itemCategory, StringComparison.OrdinalIgnoreCase)))
            {
                cScore = 1.0;
            }

            // 4. Weighted combination.
            var score =
                (vScore * WeightVector) +
                (dScore * WeightDistance) +
                (cScore * WeightCategory);

            return score;
        }

        private static double Clamp01(double value)
        {
            if (value < 0) return 0;
            if (value > 1) return 1;
            return value;
        }
    }
}