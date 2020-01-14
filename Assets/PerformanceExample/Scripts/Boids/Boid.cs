using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace PerformanceExample
{
    public class Boid : MonoBehaviour, IChunkEntity
    {
        public BoidBehaviour Settings;
        [HideInInspector]
        public float CurrentSpeed;

        private IChunkManager<Boid, AggregatedBoidChunk> ChunkManager;
        private Tuple<List<Boid>[], AggregatedBoidChunk> NeighborInfo;

        private Boid NearestNeighbor;
        private int BoidId;


        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            BoidId = (int)(UnityEngine.Random.value * 10000f);
        }

        /// <summary>
        /// 
        /// </summary>
        void Update()
        {
            NeighborInfo = ChunkManager.ProvideNeighborInfo(ProvidePosition());
            EvaluateNearestNeighbor(NeighborInfo.Item1);

            Process();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="neighborsLists"></param>
        /// <returns></returns>
        public void EvaluateNearestNeighbor(List<Boid>[] neighborsLists)
        {
            var count = 1;
            var nearestDistance = 1000000f;
            NearestNeighbor = null;

            for (int listIndex = 0; listIndex < neighborsLists.Length; listIndex++)
            {
                for (int neighborIndex = 0; neighborIndex < neighborsLists[listIndex].Count(); neighborIndex++)
                {
                    var neighbor = neighborsLists[listIndex][neighborIndex];

                    if (count > Settings.NearestNeighborChecks)
                        break;
                    if (neighbor.Equals(this))
                        continue;

                    var distance = (neighbor.transform.position - transform.position).sqrMagnitude;
                    if (distance < nearestDistance)
                    {
                        NearestNeighbor = neighbor;
                        nearestDistance = distance;
                    }

                    count++;
                }

                if (count > Settings.NearestNeighborChecks)
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Process()
        {
            var newDirection = transform.forward;
            CurrentSpeed = Settings.BaseSpeed + Settings.RandomSpeedAmplitude * Mathf.PerlinNoise(Time.time * Settings.RandomSpeedFrequency, BoidId);

            // Target
            newDirection += (Settings.Target - transform.position).normalized * Settings.TargetWeight;

            if (NearestNeighbor != null)
            {
                var aggregatedType = NeighborInfo.Item2.BoidTypes[Settings.GetInstanceID()];
                var nearestNeighborDiff = (transform.position - NearestNeighbor.transform.position);
                var centerDiff = aggregatedType.Position - transform.position;
                var centerDiffNormalized = centerDiff.normalized;


                // Seperation
                newDirection += Settings.SeperationWeight * nearestNeighborDiff.normalized / Mathf.Max(0.001f, nearestNeighborDiff.sqrMagnitude);

                // Alignment
                newDirection += Settings.AlignmentWeight * aggregatedType.Direction / Mathf.Max(0.001f, centerDiff.sqrMagnitude);

                // Cohesion
                newDirection += Settings.CohesionWeight * centerDiffNormalized;
                CurrentSpeed = Mathf.Lerp(CurrentSpeed, aggregatedType.Speed, Mathf.Min(1f, 1 / centerDiff.sqrMagnitude));

                // Relation
                foreach (var relation in Settings.Relations)
                {
                    var boidType = relation.Behaviour.GetInstanceID();
                    if (NeighborInfo.Item2.BoidTypes.ContainsKey(boidType))
                    {
                        var otherInfo = NeighborInfo.Item2.BoidTypes[boidType];
                        var relationdiff = (otherInfo.Position - transform.position);

                        var neighbourDirection = relationdiff.normalized / relationdiff.sqrMagnitude * relation.Attraction;
                        newDirection += neighbourDirection;
                        CurrentSpeed = Mathf.Lerp(Mathf.Max(CurrentSpeed, -CurrentSpeed * relation.Attraction), CurrentSpeed, Mathf.Max(1f, neighbourDirection.sqrMagnitude));
                    }
                }
            }

            transform.forward = Vector3.Lerp(transform.forward, newDirection, Settings.DirectionReactionSpeed * Time.deltaTime);
            transform.position += CurrentSpeed * Time.deltaTime * transform.forward;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TAggregation"></typeparam>
        /// <param name="manager"></param>
        public void SetChunkManager<TEntity, TAggregation>(IChunkManager<TEntity, TAggregation> manager) where TEntity : IChunkEntity
        {
            ChunkManager = (IChunkManager<Boid, AggregatedBoidChunk>)manager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public System.Numerics.Vector3 ProvidePosition()
        {
            var p = transform.position;
            return new System.Numerics.Vector3(p.x, p.y, p.z);
        }


        /// <summary>
        /// 
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (ChunkManager == null)
                return;


            ChunkManager.UpdateChunks();
            var info = ChunkManager.ProvideNeighborInfo(ProvidePosition());

            foreach (var list in info.Item1)
            {
                foreach (var neighbor in list)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(transform.position, neighbor.transform.position);
                }
            }

            if (info.Item2.BoidTypes.ContainsKey(Settings.GetInstanceID()))
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, info.Item2.BoidTypes[Settings.GetInstanceID()].Position);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + info.Item2.BoidTypes[Settings.GetInstanceID()].Direction);
            }
        }
    }
}