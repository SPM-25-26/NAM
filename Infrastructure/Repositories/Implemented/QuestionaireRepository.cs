using Domain.Entities;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Implemented
{
    public class QuestionaireRepository(ApplicationDbContext context) : Repository<Questionaire, Guid>(context), IQuestionaireRepository
    {
        public async Task<bool> UpdateAsync(Questionaire questionaire, User user, CancellationToken cancellationToken = default)
        {
            user.Questionaire = questionaire;
            context.Users.Update(user);
            var result = await context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }
    }
}
