using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PerformanceExample
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Performance/BoidBehaviour")]
    public class BoidBehaviour : ScriptableObject
    {
        [Serializable]
        public struct Relation
        {
            public BoidBehaviour Behaviour;
            public float Attraction;
        }


        [Header("Performance...")]
        public int NearestNeighborChecks;

        [Header("General...")]
        public float DirectionReactionSpeed;
        public float BaseSpeed;
        public float RandomSpeedAmplitude;
        public float RandomSpeedFrequency;

        [Header("Influences...")]
        public float TargetWeight;
        public float SeperationWeight;
        public float AlignmentWeight;
        public float CohesionWeight;
        public Vector3 Target;
        public Relation[] Relations;
    }
}