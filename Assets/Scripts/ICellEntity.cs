using System.Numerics;

public interface ICellEntity
{
    void SetCellManager<TCell, TAggregation>(ICellManager<TCell, TAggregation> manager) where TCell : ICellEntity;
    Vector3 ProvideCellPosition();
}