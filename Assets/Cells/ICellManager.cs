using System.Collections.Generic;
using System.Numerics;

public interface ICellManager<TCell, TAggregation>
    where TCell : ICellEntity
{
    float CellSize { get; set; }

    void Add(TCell entity);
    void UpdateLists();
    IEnumerable<TCell>[] GetNeighbourEntities(Vector3 position);
    TAggregation GetNeighborAggregation(Vector3 position);
}