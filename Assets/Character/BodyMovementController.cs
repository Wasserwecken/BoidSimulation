using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BodyMovementController : MonoBehaviour, IBodyMovement
{
    public CharacterController CController;

    private Vector3 Velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Move(Vector2 direction)
    {
        throw new System.NotImplementedException();
    }

    public void Jump()
    {
        throw new System.NotImplementedException();
    }

    public void Crouch()
    {
        throw new System.NotImplementedException();
    }
}
