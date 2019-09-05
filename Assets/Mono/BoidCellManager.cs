using UnityEngine;

public class BoidCellManager : MonoBehaviour
{
    public float CellSize;

    private ICellManager<Boid, AggregatedBoidData> CellManager = new CellManager<Boid, AggregatedBoidData>();


    void Start()
    {
        CellManager.CellSize = CellSize;
    }

    void Update()
    {
        CellManager.UpdateLists();
    }

    public void Add(Boid boid)
    {
        CellManager.Add(boid);
    }
}