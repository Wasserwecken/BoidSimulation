using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BoidSettings")]
public class BoidSettings : ScriptableObject
{
    [Header("General...")]
    [Range(0f, 1f)]
    public float DirectionReactionSpeed;
    public float MovementSpeed;
    public float ViewRange;

    [Header("Rules...")]
    public bool UseSeperation;
    public float SeperationWeight;

    public bool UseAlignment;
    public float AlignmentWeight;

    public bool UseCohesion;
    public float CohesionWeight;

    public bool UseTarget;
    public Vector3 Target;
    public float TargetWeight;
}