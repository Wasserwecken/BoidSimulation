using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoids : MonoBehaviour
{
    public Boid BoidPrefab;
    public BoidCellManager CellManager;
    public int Count;
    public float Radius;
    

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        for (int index = 0; index < Count; index++)
        {
            Boid boid = Instantiate(BoidPrefab, Random.insideUnitSphere * Radius + transform.position, Random.rotation, CellManager.transform);
            CellManager.Add(boid);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
