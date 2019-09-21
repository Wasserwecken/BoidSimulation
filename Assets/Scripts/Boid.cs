using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boid : MonoBehaviour, ICellEntity
{
    public BoidBehaviour Settings;
    public float CurrentSpeed;

    private ICellManager<Boid, AggregatedBoidCell> CellManager;
    private IEnumerable<Boid>[] NeighborBoidsLists;
    private AggregatedBoidCell AggregatedNeighbors;
    private Boid NearestNeighbor;
    private int BoidId;


    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        BoidId = (int)(Random.value * 10000f);
    }
    
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        var cellPosition = ProvideCellPosition();

        UnityEngine.Profiling.Profiler.BeginSample("Neighbor boids request");
        NeighborBoidsLists = CellManager.GetNeighbourEntities(cellPosition);
        UnityEngine.Profiling.Profiler.EndSample();


        UnityEngine.Profiling.Profiler.BeginSample("Neighbor aggregations request");
        AggregatedNeighbors = CellManager.GetNeighborAggregation(cellPosition);
        UnityEngine.Profiling.Profiler.EndSample();


        UnityEngine.Profiling.Profiler.BeginSample("Nearest neighbor evaluation");
        EvaluateNearestNeighbor(NeighborBoidsLists);
        UnityEngine.Profiling.Profiler.EndSample();


        UnityEngine.Profiling.Profiler.BeginSample("Processing boid behaviour");
        Process();
        UnityEngine.Profiling.Profiler.EndSample();
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="neighborsLists"></param>
    /// <returns></returns>
    public void EvaluateNearestNeighbor(IEnumerable<IEnumerable<Boid>> neighborsLists)
    {
        var count = 1;
        var nearestDistance = 1000000f;
        NearestNeighbor = null;

        foreach(var list in neighborsLists)
        {
            foreach (var neighbor in list)
            {
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
            var aggregatedType = AggregatedNeighbors.BoidTypes[Settings.GetInstanceID()];
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
            foreach(var relation in Settings.Relations)
            {
                var boidType = relation.Behaviour.GetInstanceID();
                if (AggregatedNeighbors.BoidTypes.ContainsKey(boidType))
                {
                    var otherInfo = AggregatedNeighbors.BoidTypes[boidType];
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
    /// <typeparam name="TCell"></typeparam>
    /// <typeparam name="TAggregation"></typeparam>
    /// <param name="manager"></param>
    public void SetCellManager<TCell, TAggregation>(ICellManager<TCell, TAggregation> manager) where TCell : ICellEntity
    {
        CellManager = (ICellManager<Boid, AggregatedBoidCell>)manager;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public System.Numerics.Vector3 ProvideCellPosition()
    {
        var p = transform.position;
        return new System.Numerics.Vector3(p.x, p.y, p.z);
    }

    
    /// <summary>
    /// 
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        var neig = CellManager.GetNeighbourEntities(ProvideCellPosition());
        foreach(var list in neig)
        {
            foreach(var neighbor in list)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
            }
        }

        var agg = CellManager.GetNeighborAggregation(ProvideCellPosition());
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, agg.BoidTypes[Settings.GetInstanceID()].Position);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + agg.BoidTypes[Settings.GetInstanceID()].Direction);
    }
}
