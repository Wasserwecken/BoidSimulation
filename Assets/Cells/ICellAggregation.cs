
public interface ICellAggregation<TSelf, TEntity>
    where TSelf : ICellAggregation<TSelf, TEntity>
    where TEntity : ICellEntity
{
    int Count { get; set; }
    TSelf Aggregate(TEntity entity);
    TSelf Aggregate(TSelf otherAggregation);
    TSelf Clear();
    TSelf Finialize();
}