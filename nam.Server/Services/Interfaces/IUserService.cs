using Domain.Entities;

namespace nam.Server.Services.Interfaces
{
    public interface IUserService
    {
        Task<Questionaire?> GetQuestionaireByUserMailAsync(string userEmail, CancellationToken cancellationToken = default);
        Task<bool> UpdateQuestionaireAsync(Questionaire questionaire, string userEmail, CancellationToken cancellationToken = default);
        Task<bool> QuestionaireCompletedAsync(string userEmail, CancellationToken cancellationToken = default);
    }
}
