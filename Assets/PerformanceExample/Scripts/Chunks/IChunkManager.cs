using System;
using System.Collections.Generic;
using System.Numerics;

namespace PerformanceExample
{
    public interface IChunkManager<TEntity, TAggregation>
    where TEntity : IChunkEntity
    {
        float ChunkSize { get; set; }
        List<TEntity> Entities { get; set; }

        void Add(TEntity entity);
        void Remove(TEntity entity);
        void UpdateChunks();
        Tuple<List<TEntity>[], TAggregation> ProvideNeighborInfo(Vector3 position);
    }
}