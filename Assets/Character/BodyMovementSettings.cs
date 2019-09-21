using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BodyMovement")]
public class BodyMovementSettings : ScriptableObject
{
    public Vector3 JumpImpulse;
    public Vector2 MaximumSpeed;
    public float GravityInfluence;
    public AnimationCurve Drag;
}