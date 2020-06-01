using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float SpawnSize;
    public Boid BoidPrefab;
    public int Count;
    public bool SpawnIn3D;


    void Start()
    {
        for (int i = 0; i < Count; i++)
            SpawnBoid();
    }


    private void SpawnBoid()
    {
        Quaternion randomRotation;
        var randomPosition = transform.position + Random.insideUnitSphere;

        if (SpawnIn3D)
            randomRotation = Random.rotation;
        else
        {
            randomPosition.y = 0f;
            randomRotation = Quaternion.AngleAxis(Random.value * 360f, Vector3.up);
        }

        Instantiate(BoidPrefab, randomPosition, randomRotation, transform);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(SpawnSize, SpawnIn3D ? SpawnSize : 0f, SpawnSize) * 2f);
    }
}
