using nam.Server.Services.Interfaces.RecSys;

namespace nam.Server.Services.Implemented.RecSys
{
    /// <summary>
    /// Simple ranker that sorts items by score (descending)
    /// and selects the top N.
    /// </summary>
    public class SimpleRanker : IRanker
    {
        public List<string> RankAndSelect(IEnumerable<(Guid Id, double Score, string? EntityIdPayload)> items, int take)
        {
            return items
                .OrderByDescending(x => x.Score)
                .Take(take)
                .Select(p =>
                {

                    // 1. If there is in the payload use that, otherwise use the point ID converted to string
                    if (!string.IsNullOrWhiteSpace(p.EntityIdPayload))
                    {
                        return p.EntityIdPayload;
                    }

                    // 2. Otherwise, take the technical Guid and convert it to a string.
                    return p.Id.ToString();
                }).Distinct()
                .ToList();
        }
    }
}