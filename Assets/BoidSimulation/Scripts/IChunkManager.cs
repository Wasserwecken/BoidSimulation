using System;
using System.Collections.Generic;
using System.Numerics;

public interface IChunkManager<TEntity, TAggregation>
    where TEntity : IChunkEntity
{
    float ChunkSize { get; set; }

    void Add(TEntity entity);
    void UpdateChunks();
    Tuple<List<TEntity>[], TAggregation> ProvideNeighborInfo(Vector3 position);
}