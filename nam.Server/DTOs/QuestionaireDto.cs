namespace nam.Server.DTOs
{
    public record QuestionaireDto(
        List<string> Interest,
        List<string> TravelStyle,
        string AgeRange,
        string TravelRange,
        List<string> TravelCompanions,
        string DiscoveryMode);
}
