using DataInjection.Interfaces;
using Qdrant.Client.Grpc;

namespace DataInjection.Qdrant.Mappers
{
    internal interface IQdrantPayloadCollector : IEntityCollector<PointStruct>
    {
    }
}
