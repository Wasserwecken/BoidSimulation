using System.Collections.Generic;
using System.Numerics;

public interface IChunkManager<TEntity, TAggregation>
    where TEntity : IChunkEntity
{
    float ChunkSize { get; set; }

    void Add(TEntity entity);
    void UpdateChunks();
    IEnumerable<TEntity>[] GetNeighbourEntities(Vector3 position);
    TAggregation GetNeighborAggregation(Vector3 position);
}