using Domain.Entities;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IQuestionaireRepository : IRepository<Questionaire, Guid>
    {
        Task<bool> UpdateAsync(Questionaire questionaire, User user, CancellationToken cancellationToken = default);
    }
}
