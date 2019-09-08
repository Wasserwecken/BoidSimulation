using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boid : MonoBehaviour, ICellEntity
{
    public BoidSettings Settings;

    private ICellManager<Boid, AggregatedBoidCell> CellManager;
    private IEnumerable<Boid>[] NeighborBoidsLists;
    private AggregatedBoidCell AggregatedNeighbors;
    private Boid NearestNeighbor;
    
    
    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        var cellPosition = ProvideCellPosition();

        UnityEngine.Profiling.Profiler.BeginSample("Neighbor boids");
        NeighborBoidsLists = CellManager.GetNeighbourEntities(cellPosition);
        UnityEngine.Profiling.Profiler.EndSample();


        UnityEngine.Profiling.Profiler.BeginSample("Neighbor aggregations");
        AggregatedNeighbors = CellManager.GetNeighborAggregation(cellPosition);
        UnityEngine.Profiling.Profiler.EndSample();


        UnityEngine.Profiling.Profiler.BeginSample("Nearest neighbor");
        EvaluateNearestNeighbor(NeighborBoidsLists);
        UnityEngine.Profiling.Profiler.EndSample();


        UnityEngine.Profiling.Profiler.BeginSample("Processing data");
        ProcessDirection();
        ProcessSpeed();
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
        NearestNeighbor = null;
        var nearestDistance = 1000000f;

        foreach(var list in neighborsLists)
        {
            foreach (var neighbor in list)
            {
                if (count > Settings.NearestNeighborLimit)
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

            if (count > Settings.NearestNeighborLimit)
                break;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ProcessDirection()
    {
        var newDirection = transform.forward;
        
        // Target
        newDirection += (Settings.Target - transform.position).normalized * Settings.TargetWeight;

        if (NearestNeighbor != null && AggregatedNeighbors.Count > 1)
        {
            var nearestNeighborDiff = (transform.position - NearestNeighbor.transform.position);
            var centerDiff = AggregatedNeighbors.BoidTypeData[Settings.GetInstanceID()].Position - transform.position;
            var centerDiffNormalized = centerDiff.normalized;
            
            
            // Seperation
            newDirection += Settings.SeperationWeight * nearestNeighborDiff.normalized / Mathf.Max(0.001f, nearestNeighborDiff.sqrMagnitude);

            // Alignment
            newDirection += Settings.AlignmentWeight * AggregatedNeighbors.BoidTypeData[Settings.GetInstanceID()].Direction / Mathf.Max(0.001f, centerDiff.sqrMagnitude);

            // Cohesion
            newDirection += Settings.CohesionWeight * centerDiffNormalized;

            // Relation
            foreach(var relation in Settings.Relations)
            {
                var boidType = relation.Settings.GetInstanceID();
                if (AggregatedNeighbors.BoidTypeData.ContainsKey(boidType))
                {
                    var otherInfo = AggregatedNeighbors.BoidTypeData[boidType];
                    var diff = (otherInfo.Position - transform.position);
                    newDirection += diff.normalized / diff.sqrMagnitude * relation.Friendly;
                }
            }
        }
        
        transform.forward = Vector3.Lerp(transform.forward, newDirection, Settings.DirectionReactionSpeed * Time.deltaTime);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void ProcessSpeed()
    {
        transform.position += Settings.Speed * Time.deltaTime * transform.forward;
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
        var neighborLists = CellManager.GetNeighbourEntities(ProvideCellPosition());
        foreach(var list in neighborLists)
        {
            foreach(var neighbor in list)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position, neighbor.transform.position);
            }
        }

        var agg = CellManager.GetNeighborAggregation(ProvideCellPosition());
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, agg.BoidTypeData[Settings.GetInstanceID()].Position);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + agg.BoidTypeData[Settings.GetInstanceID()].Direction);
    }
}
