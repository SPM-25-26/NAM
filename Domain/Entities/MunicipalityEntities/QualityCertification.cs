
using System.Text.Json.Serialization;

namespace Domain.Entities.MunicipalityEntities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QualityCertification
    {
        Nessuna,
        Doc,
        Docg,
        Dop,
        Igt,
        Igp,
        Stg,
        Biologico
    }
}
