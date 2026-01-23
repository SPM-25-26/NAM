

using System.Text.Json.Serialization;

namespace Domain.Entities.MunicipalityEntities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EntityStatus
    {
        Draft, 
        Completed, 
        ReadyToPublish, 
        Published
    }
}
