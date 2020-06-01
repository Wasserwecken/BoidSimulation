using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public BoidSettings Settings;
    public float movementRadius;
    public float movementSpeed;

    private Vector3 origin;


    private void Start()
    {
        origin = transform.position;
    }

    void Update()
    {
        var time = Time.realtimeSinceStartup * movementSpeed;
        var offset = new Vector3(
            Mathf.PerlinNoise(time, 0) * 2f - 1f,
            0,
            Mathf.PerlinNoise(0, time) * 2f - 1f
        );
        transform.position = origin + offset * movementRadius;

        Settings.Target = transform.position;
    }
}
