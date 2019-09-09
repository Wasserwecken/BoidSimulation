using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Boid/Settings")]
public class BoidSettings : ScriptableObject
{
    [Serializable]
    public struct Relation
    {
        public BoidSettings Settings;
        public float Friendly;
    }

    public int NearestNeighborLimit;
    public float Speed;
    public float DirectionReactionSpeed;
    public float TargetWeight;
    public float SeperationWeight;
    public float AlignmentWeight;
    public float CohesionWeight;
    public Vector3 Target;
    public List<Relation> Relations;
}

