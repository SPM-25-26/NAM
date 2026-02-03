namespace Infrastructure.Repositories.Interfaces
{
    public interface IEntitySource
    {
        string EntityName { get; }

        Task<string> GetContentAsync(string id, CancellationToken ct = default);
    }
}
