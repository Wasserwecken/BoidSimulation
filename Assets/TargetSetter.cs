using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSetter : MonoBehaviour
{
    public BoidSettings[] Settings;



    // Update is called once per frame
    void Update()
    {
        foreach(var setting in Settings)
            setting.Target = transform.position;        
    }
}
