using DataInjection.Interfaces;
using DataInjection.Qdrant.Data;

namespace DataInjection.Qdrant.Mappers
{
    internal interface IQdrantPayloadCollector : IEntityCollector<POIEntity>
    {
    }
}
