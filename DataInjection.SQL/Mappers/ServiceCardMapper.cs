using DataInjection.Core.Interfaces;
using DataInjection.SQL.DTOs;
using Domain.Entities.MunicipalityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInjection.SQL.Mappers
{
    public class ServiceCardMapper : IDtoMapper<List<ServiceCardDto>, List<ServiceCard>>
    {
        public List<ServiceCard> MapToEntity(List<ServiceCardDto> dtos)
        {
            if (dtos == null || dtos.Count == 0)
                return [];

            return dtos.Where(dto => dto is not null)
                .Select(dto => new ServiceCard
                {
                    EntityId = Guid.TryParse(dto.EntityId, out Guid entityId) ? entityId : Guid.NewGuid(),
                    EntityName = dto.EntityName ?? string.Empty,
                    ImagePath = dto.ImagePath ?? string.Empty,
                    BadgeText = dto.BadgeText ?? string.Empty,
                    Address = dto.Address ?? string.Empty
                }).ToList();
        }
    }
}
