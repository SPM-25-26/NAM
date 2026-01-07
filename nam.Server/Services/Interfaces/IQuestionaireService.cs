using Domain.Entities;

namespace nam.Server.Services.Interfaces
{
    public interface IQuestionaireService
    {
        Task<Questionaire?> GetByUserMailAsync(string userEmail, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(Questionaire questionaire, string userEmail, CancellationToken cancellationToken = default);
    }
}
