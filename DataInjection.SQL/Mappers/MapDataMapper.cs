using DataInjection.Core.Interfaces;
using DataInjection.SQL.DTOs;
using Domain.Entities.MunicipalityEntities;

namespace DataInjection.SQL.Mappers
{
    public class MapDataMapper : IDtoMapper<MapDataDto, MapData>
    {
        public MapData MapToEntity(MapDataDto dto)
        {
            var entity = new MapData
            {
                // NB: viene impostato dal chiamante (collector) usando il parametro della query
                Name = string.Empty,
                CenterLatitude = dto.CenterLatitude ?? 0,
                CenterLongitude = dto.CenterLongitude ?? 0,
            };

            if (dto?.Markers == null || dto.Markers.Count == 0)
                return entity;

            foreach (var m in dto.Markers)
            {
                if (m is null) continue;

                entity.Marker.Add(new MapMarker
                {
                    Id= Guid.TryParse(m.Id, out Guid Id) ? Id : Guid.NewGuid(),
                    ImagePath = m.ImagePath ?? string.Empty,
                    Name = m.Name ?? string.Empty,
                    Typology = m.Typology ?? string.Empty,
                    Address = m.Address ?? string.Empty,
                    Latitude = m.Latitude,
                    Longitude = m.Longitude
                });
            }

            return entity;
        }
    }
}