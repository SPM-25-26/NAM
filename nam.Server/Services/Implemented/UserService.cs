using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.AI;
using Microsoft.SemanticKernel.ChatCompletion;
using nam.Server.Services.Interfaces;

namespace nam.Server.Services.Implemented
{
    public class UserService(
        IUnitOfWork unitOfWork,
        IEmbeddingGenerator<string, Embedding<float>> embedder,
        IChatCompletionService chatService
        ) : IUserService
    {
        private readonly QuestionaireEmbeddingStringImprover questionaireEmbeddingImprover = new(chatService);

        public async Task<Questionaire?> GetQuestionaireByUserMailAsync(string userEmail, CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.Users.GetByEmailAsync(userEmail, cancellationToken)
                ?? throw new ArgumentException($"Nessun utente trovato con l'email '{userEmail}'.", nameof(userEmail));
            return user.Questionaire;
        }

        public async Task<bool> UpdateQuestionaireAsync(Questionaire questionaire, string userEmail, CancellationToken cancellationToken = default)
        {
            var questionaireImproved = await questionaireEmbeddingImprover.ImproveEmbeddingStringAsync(questionaire.ToEmbeddingString(), cancellationToken);
            var vector = await embedder.GenerateVectorAsync(questionaireImproved, cancellationToken: cancellationToken);
            questionaire.Vector = vector.ToArray();
            var result = await unitOfWork.Users.UpdateQuestionaireByEmailAsync(questionaire, userEmail, cancellationToken);
            return result;
        }

        public async Task<bool> QuestionaireCompletedAsync(string userEmail, CancellationToken cancellationToken = default)
        {
            var user = await unitOfWork.Users.GetByEmailAsync(userEmail, cancellationToken)
                ?? throw new ArgumentException($"Nessun utente trovato con l'email '{userEmail}'.", nameof(userEmail));

            var q = user.Questionaire;

            if (q is null) return false;

            return (q.Interest != null && q.Interest.Count > 0) ||
                   (q.TravelStyle != null && q.TravelStyle.Count > 0) ||
                   !string.IsNullOrWhiteSpace(q.AgeRange) ||
                   !string.IsNullOrWhiteSpace(q.TravelRange) ||
                   (q.TravelCompanions != null && q.TravelCompanions.Count > 0) ||
                   !string.IsNullOrWhiteSpace(q.DiscoveryMode);
        }
    }
}
