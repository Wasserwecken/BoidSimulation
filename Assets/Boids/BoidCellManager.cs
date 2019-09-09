using UnityEngine;

public class BoidCellManager : MonoBehaviour
{
    public float CellSize;

    private ICellManager<Boid, AggregatedBoidCell> CellManager = new CellManager<Boid, AggregatedBoidCell>();



    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        CellManager.CellSize = CellSize;
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        CellManager.UpdateCells();
        CellManager.UpdateCellAggregates();
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