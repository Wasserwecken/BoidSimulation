
public interface IChunkAggregation<TSelf, TEntity>
    where TSelf : IChunkAggregation<TSelf, TEntity>
    where TEntity : IChunkEntity
{
    int Count { get; set; }
    TSelf Include(TEntity entity);
    TSelf Combine(TSelf otherAggregation);
    TSelf Clear();
    TSelf Finialize();
}