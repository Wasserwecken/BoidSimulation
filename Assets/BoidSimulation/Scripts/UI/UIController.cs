using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Info source...")]
    public BoidChunkManager BoidManager;

    [Header("UI elements...")]
    public Text DesiredFPSLabel;
    public Text CurrentFPSLabel;
    public Text BoidCountLabel;
    public Button ExitButton;
    public Button FPSDecreaseButton;
    public Button FPSIncreaseButton;
    

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        ExitButton.onClick.AddListener(() => { Application.Quit(); });
        FPSDecreaseButton.onClick.AddListener(() => { BoidManager.DesiredFPS--; });
        FPSIncreaseButton.onClick.AddListener(() => { BoidManager.DesiredFPS++; });
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        DesiredFPSLabel.text = $"{BoidManager.DesiredFPS} FPS";
        CurrentFPSLabel.text = $"{BoidManager.FPS} FPS";
        BoidCountLabel.text = $"{BoidManager.BoidCount} Boids";
    }
}
