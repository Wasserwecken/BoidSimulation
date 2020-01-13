using System.Numerics;

public interface IChunkEntity
{
    void SetChunkManager<TChunk, TAggregation>(IChunkManager<TChunk, TAggregation> manager) where TChunk : IChunkEntity;
    Vector3 ProvidePosition();
}