using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSetter : MonoBehaviour
{
    public BoidSettings[] Settings;


    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        foreach(var setting in Settings)
            setting.Target = transform.position;        
    }
}
