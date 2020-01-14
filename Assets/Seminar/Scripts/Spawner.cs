using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float SpawnSize;
    public Boid BoidPrefab;
    public int Count;
    public bool SpawnIn3D;


    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        Count.Times(SpawnBoid);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(SpawnSize, SpawnIn3D ? SpawnSize : 0f, SpawnSize) * 2f);
    }


    private void SpawnBoid()
    {
        var randomPosition = new Vector3(
                (Random.value * 2f - 1f) * SpawnSize,
                SpawnIn3D ? (Random.value * 2f - 1f) : 0f,
                (Random.value * 2f - 1f) * SpawnSize
            );

        Quaternion randomRotation;
        if (SpawnIn3D)
            randomRotation = Random.rotation;
        else
            randomRotation = Quaternion.AngleAxis(Random.value * 360f, Vector3.up);

        Instantiate(BoidPrefab, randomPosition, randomRotation);
    }
}
