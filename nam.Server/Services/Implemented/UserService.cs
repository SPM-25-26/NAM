using Domain.Entities;
using Infrastructure.UnitOfWork;
using nam.Server.Services.Interfaces;

namespace nam.Server.Services.Implemented
{
    public class UserService(IUnitOfWork unitOfWork) : IUserService
    {
        public async Task<Questionaire?> GetQuestionaireByUserMailAsync(string userEmail, CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.Users.GetByEmailAsync(userEmail, cancellationToken)
                ?? throw new ArgumentException($"Nessun utente trovato con l'email '{userEmail}'.", nameof(userEmail));
            return user.Questionaire;
        }

        public async Task<bool> UpdateQuestionaireAsync(Questionaire questionaire, string userEmail, CancellationToken cancellationToken = default)
        {
            var result = await unitOfWork.Users.UpdateQuestionaireByEmailAsync(questionaire, userEmail, cancellationToken);
            return result;
        }
    }
}
