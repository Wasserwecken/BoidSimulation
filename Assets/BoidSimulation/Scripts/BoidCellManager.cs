using UnityEngine;

public class BoidCellManager : MonoBehaviour
{
    public float CellSize;

    private IChunkManager<Boid, AggregatedBoidChunk> CellManager = new ChunkManager<Boid, AggregatedBoidChunk>();



    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        CellManager.ChunkSize = CellSize;
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        CellManager.UpdateChunks();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="boid"></param>
    public void Add(Boid boid)
    {
        CellManager.Add(boid);
    }
}