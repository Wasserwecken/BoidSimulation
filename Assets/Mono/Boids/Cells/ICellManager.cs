using System.Collections.Generic;
using System.Numerics;

public interface ICellManager<TEntity, TAggregation>
    where TEntity : ICellEntity
{
    float CellSize { get; set; }

    void Add(TEntity entity);
    void UpdateLists();
    IEnumerable<TEntity>[] GetNeighbourEntities(Vector3 position);
    TAggregation GetNeighborAggregation(Vector3 position);
}