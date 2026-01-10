namespace nam.Server.Services.Interfaces.RecSys
{
    public interface IScorer
    {
        /// <summary>
        /// Calculates a unified score based on vector similarity, distance, and category relevance.
        /// </summary>
        /// <param name="vectorScore">Semantic similarity score (from Qdrant)</param>
        /// <param name="distanceKm">Distance from user in km</param>
        /// <param name="itemCategory">Category of the specific POI</param>
        /// <param name="preferredCategories">List of categories the user is interested in</param>
        /// <returns>Final weighted score</returns>
        double CalculateScore(double vectorScore, double? distanceKm, string itemCategory, List<string> preferredCategories, double searchRadiusKm);
    }
}
