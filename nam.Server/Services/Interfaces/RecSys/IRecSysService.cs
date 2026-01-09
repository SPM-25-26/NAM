namespace nam.Server.Services.Interfaces.RecSys
{
    public interface IRecSysService
    {
        /// <summary>
        /// Generates personalized recommendations based on the user's questionnaire and location.
        /// Uses a multi-stage fallback strategy.
        /// </summary>
        Task<List<Guid>> GetRecommendationsAsync(string userEmail, double? realLat, double? realLon);
    }
}
