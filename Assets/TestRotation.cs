using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    public Transform T;
    public float speed;
    
    void Start()
    {
    }
    
    void Update()
    {
        UnityEngine.Profiling.Profiler.BeginSample("Rotation");

        T.Rotate(new Vector3(0f, Time.deltaTime * speed, 0f));

        UnityEngine.Profiling.Profiler.EndSample();
    }
}
