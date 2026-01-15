namespace nam.Server.Services.Interfaces.RecSys
{
    /// <summary>
    /// Provides a way to rank and select a subset of scored items.
    /// </summary>
    public interface IRanker
    {
        /// <summary>
        /// Ranks items by their score and returns the top N identifiers.
        /// </summary>
        /// <param name="items">Sequence of (Id, Score) pairs.</param>
        /// <param name="take">Number of items to return.</param>
        /// <returns>List of item identifiers ordered by descending score.</returns>
        List<string> RankAndSelect(IEnumerable<(Guid Id, double Score, string? EntityIdPayload)> items, int take);
    }
}