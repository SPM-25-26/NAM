using nam.Server.Services.Interfaces.RecSys;

namespace nam.Server.Services.Implemented.RecSys
{
    /// <summary>
    /// Simple ranker that sorts items by score (descending)
    /// and selects the top N.
    /// </summary>
    public class SimpleRanker : IRanker
    {
        public List<Guid> RankAndSelect(IEnumerable<(Guid Id, double Score)> items, int take)
        {
            return items
                .OrderByDescending(x => x.Score)
                .Take(take)
                .Select(x => x.Id)
                .ToList();
        }
    }
}