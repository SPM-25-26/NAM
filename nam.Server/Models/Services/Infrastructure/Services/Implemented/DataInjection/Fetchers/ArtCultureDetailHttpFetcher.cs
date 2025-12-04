using nam.Server.Models.DTOs.MunicipalityInjection;
using nam.Server.Models.Entities.MunicipalityEntities;

namespace nam.Server.Models.Services.Infrastructure.Services.Implemented.DataInjection.Fetchers
{
    public class ArtCultureDetailHttpFetcher(HttpClient httpClient, ILogger logger, IConfiguration configuration, Dictionary<string, string?> query) : AbstractHttpFetcher<ArtCultureNatureDetailDto, ArtCultureNatureDetail>(httpClient, logger, configuration, query)
    {
        protected override string GetEndpointUrl()
        {
            return "api/art-culture/detail";
        }

        protected override ArtCultureNatureDetail MapToEntity(ArtCultureNatureDetailDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
