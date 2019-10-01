using UnityEngine;

public class BoidChunkManager : MonoBehaviour
{
    public float CellSize;

    private IChunkManager<Boid, AggregatedBoidChunk> ChunkManager = new ChunkManager<Boid, AggregatedBoidChunk>();



    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        ChunkManager.ChunkSize = CellSize;
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        ChunkManager.UpdateChunks();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="boid"></param>
    public void Add(Boid boid)
    {
        ChunkManager.Add(boid);
    }
}