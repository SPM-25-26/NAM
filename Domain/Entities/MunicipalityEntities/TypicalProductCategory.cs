
using System.Text.Json.Serialization;

namespace Domain.Entities.MunicipalityEntities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TypicalProductCategory
    {
        Cibo,
        Bevanda,
        ProdottoArtigianale,
        ProdottoIndustriale
    }
}
