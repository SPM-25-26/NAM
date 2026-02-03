using Domain.Entities.MunicipalityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces.MunicipalityEntities
{
    public interface IServiceRepository : IRepository<ServiceCard, Guid>, IMunicipalityEntityRepository<ServiceCard, ServiceDetail, Guid>
    {
    }
}
