using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;
using nam.Server.Models.Services.Infrastructure.Services.Interfaces.DataInjection;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Mappers
{
    public class MunicipalityHomeInfoMapper : IDtoMapper<MunicipalityHomeInfoDto, MunicipalityHomeInfo>
    {
        public MunicipalityHomeInfo MapToEntity(MunicipalityHomeInfoDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new MunicipalityHomeInfo
            {
                LegalName = string.IsNullOrWhiteSpace(dto.LegalName) ? null : dto.LegalName!.Trim(),
                Name = string.IsNullOrWhiteSpace(dto.Name) ? null : dto.Name!.Trim(),
                Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description!.Trim(),
                Contacts = MapContactInfo(dto.Contacts),
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                LogoPath = string.IsNullOrWhiteSpace(dto.LogoPath) ? null : dto.LogoPath!.Trim(),
                HomeImages = dto.HomeImages?.Where(img => !string.IsNullOrWhiteSpace(img)).Select(img => img.Trim()).ToList() ?? [],
                Events = dto.Events?.Where(e => e is not null).Select(MapFeatureCard).ToList() ?? [],
                ArticlesAndPaths = dto.ArticlesAndPaths?.Where(a => a is not null).Select(MapFeatureCard).ToList() ?? [],
                PanoramaPath = string.IsNullOrWhiteSpace(dto.PanoramaPath) ? null : dto.PanoramaPath!.Trim(),
                PanoramaWidth = dto.PanoramaWidth,
                VirtualTourUrls = dto.VirtualTourUrls?.Where(url => !string.IsNullOrWhiteSpace(url)).Select(url => url.Trim()).ToList() ?? [],
                NameAndProvince = string.IsNullOrWhiteSpace(dto.NameAndProvince) ? null : dto.NameAndProvince!.Trim()
            };
        }

        private MunicipalityHomeContactInfo? MapContactInfo(MunicipalityHomeContactInfoDto? dto)
        {
            if (dto == null)
                return null;

            return new MunicipalityHomeContactInfo
            {
                Email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email!.Trim(),
                Telephone = string.IsNullOrWhiteSpace(dto.Telephone) ? null : dto.Telephone!.Trim(),
                Website = string.IsNullOrWhiteSpace(dto.Website) ? null : dto.Website!.Trim(),
                Facebook = string.IsNullOrWhiteSpace(dto.Facebook) ? null : dto.Facebook!.Trim(),
                Instagram = string.IsNullOrWhiteSpace(dto.Instagram) ? null : dto.Instagram!.Trim()
            };
        }

        private FeatureCard MapFeatureCard(FeatureCardDto dto)
        {
            return new FeatureCard
            {
                EntityId = string.IsNullOrWhiteSpace(dto.EntityId) ? null : dto.EntityId!.Trim(),
                Title = string.IsNullOrWhiteSpace(dto.Title) ? null : dto.Title!.Trim(),
                Category = dto.Category ?? default,
                ImagePath = string.IsNullOrWhiteSpace(dto.ImagePath) ? null : dto.ImagePath!.Trim(),
                ExtraInfo = string.IsNullOrWhiteSpace(dto.ExtraInfo) ? null : dto.ExtraInfo!.Trim()
            };
        }
    }
}