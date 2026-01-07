namespace DataInjection.SQL.DTOs
{
    public class ArticleCardDto
    {
        public string entityId { get; set; }

        public required string EntityName { get; set; }

        public required string BadgeText { get; set; }

        public required string ImagePath { get; set; }

        public string? Address { get; set; }
    }
}
