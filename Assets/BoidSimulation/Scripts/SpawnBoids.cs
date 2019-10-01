using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoids : MonoBehaviour
{
    public List<SpawnEntry> SpawnEntries;
    public BoidCellManager ResponsibleManager;
    public float AreaRadius = 10;
    

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        foreach(var entry in SpawnEntries)
        {
            entry.Count.Times(() =>
            {
                var startPosition = Random.insideUnitSphere * AreaRadius + transform.position;
                Boid boid = Instantiate(
                        entry.Prefab,
                        startPosition,
                        Random.rotation,
                        ResponsibleManager.transform
                );
                ResponsibleManager.Add(boid);
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AreaRadius);
    }


    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public struct SpawnEntry
    {
        public Boid Prefab;
        public int Count;
    }
}
