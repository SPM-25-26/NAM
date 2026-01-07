using Domain.Entities;
using Infrastructure.UnitOfWork;
using nam.Server.Services.Interfaces;

namespace nam.Server.Services.Implemented
{
    public class QuestionaireService(IUnitOfWork unitOfWork) : IQuestionaireService
    {
        public async Task<Questionaire?> GetByUserMailAsync(string userEmail, CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.Users.GetByEmailAsync(userEmail, cancellationToken)
                ?? throw new ArgumentException($"Nessun utente trovato con l'email '{userEmail}'.", nameof(userEmail));
            return user.Questionaire;
        }

        public async Task<bool> UpdateAsync(Questionaire questionaire, string userEmail, CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.Users.GetByEmailAsync(userEmail, cancellationToken)
                ?? throw new ArgumentException($"Nessun utente trovato con l'email '{userEmail}'.", nameof(userEmail));
            var result = await unitOfWork.Questionaires.UpdateAsync(questionaire, user, cancellationToken);
            return result;
        }
    }
}
