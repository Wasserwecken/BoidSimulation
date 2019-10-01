using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoReader : MonoBehaviour
{
    [Header("Info source...")]
    public BoidChunkManager BoidManager;

    [Header("UI elements...")]
    public Text FPSLabel;
    public Text BoidCountLabel;
    

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        FPSLabel.text = $"{BoidManager.FPS} FPS";
        BoidCountLabel.text = $"{BoidManager.BoidCount} Boids";
    }
}
