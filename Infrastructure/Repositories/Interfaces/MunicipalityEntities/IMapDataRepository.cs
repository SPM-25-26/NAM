using Domain.Entities.MunicipalityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IMapDataRepository : IRepository<MapData, string>
    {
        Task<MapData> GetByMunicipalityNameAsync(string municipalityName, CancellationToken cancellationToken = default);
    }
}
