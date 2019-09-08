
public interface ICellAggregation<TSelf, TEntity>
    where TSelf : ICellAggregation<TSelf, TEntity>
    where TEntity : ICellEntity
{
    int Count { get; set; }
    TSelf Add(TEntity entity);
    TSelf Combine(TSelf otherAggregation);
    TSelf Clear();
    TSelf Finialize();
}