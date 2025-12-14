using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces;
using nam.Server.Models.Services.Infrastructure.Repositories.Interfaces.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IArtCultureRepository ArtCulture { get; }
        IArticleRepository Article { get; }
        IMunicipalityCardRepository MunicipalityCard { get; }
        INatureRepository Nature { get; }
        IOrganizationRepository Organization { get; }
        IPublicEventRepository PublicEvent { get; }
        IEntertainmentLeisureRepository EntertainmentLeisure { get; }

        Task CompleteAsync();
    }
}
