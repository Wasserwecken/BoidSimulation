using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public BoidSettings Settings;

    void Update()
    {
        Settings.Target = transform.position;
    }
}
