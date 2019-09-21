using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputCatcher : MonoBehaviour
{
    public BodyMovementController MovementController;


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
        WSAD();
        Jump();
    }


    /// <summary>
    /// 
    /// </summary>
    private void WSAD()
    {
        var movement = Vector2.zero;

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        MovementController.Move(movement);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Jump()
    {
        if (Input.GetButton("Jump"))
            MovementController.Jump();
    }
}
