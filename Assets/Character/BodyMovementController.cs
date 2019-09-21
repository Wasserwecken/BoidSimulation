using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BodyMovementController : MonoBehaviour, IBodyMovement
{
    public CharacterController Body;
    public BodyMovementSettings Settings;

    private Vector3 Velocity;
    private MovementModes MovementMode;
    

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
        AddVelocity(Settings.GravityInfluence * Physics.gravity);

        ProcessVelocity();
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"></param>
    public void Move(Vector2 direction)
    {
        var input = new Vector3(direction.x, 0f, direction.y);
        AddVelocity(input);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode"></param>
    public void SetMovementMode(MovementModes mode)
    {
        MovementMode = mode;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Jump()
    {
        if (Body.isGrounded)
            Velocity += Settings.JumpImpulse;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="change"></param>
    private void AddVelocity(Vector3 change)
    {
        Velocity += Time.deltaTime * change;
    }

    /// <summary>
    /// 
    /// </summary>
    private void ProcessVelocity()
    {
        if (Body.isGrounded)
            Velocity.Set(
                    Velocity.x,
                    Mathf.Max(0, Velocity.y),
                    Velocity.z
            );

        var border = Settings.MaximumSpeed * new Vector2(Mathf.Abs(Velocity.x), Mathf.Abs(Velocity.z)).normalized;
        Velocity.Set(
                Mathf.Min(border.x, Mathf.Abs(Velocity.x)) * Mathf.Sign(Velocity.x),
                Velocity.y,
                Mathf.Min(border.y, Mathf.Abs(Velocity.z) * Mathf.Sign(Velocity.z))
        );

        var foo = new Vector2(Velocity.x, Velocity.z) / Settings.MaximumSpeed;
        foo.Set(
            Settings.Drag.Evaluate(foo.x),
            Settings.Drag.Evaluate(foo.y)
        );

        Velocity -= new Vector3(foo.x, 0f, foo.y) * Time.deltaTime;

        Body.Move(Velocity);
    }
}
