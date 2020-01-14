using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

/// <summary>
/// https://www.red3d.com/cwr/boids/
/// </summary>
public class Boid : MonoBehaviour
{
    public BoidSettings Settings;
    private List<Boid> Neighbours;


    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        Neighbours = new List<Boid>();
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        Neighbours = GetNeighbours();
        Profiler.EndSample();

        var newDirection = transform.forward;

        if (Settings.UseTarget)
            newDirection = Target();

        if (Neighbours.Count > 0)
        {
            if (Settings.UseSeperation)
                newDirection += Seperation(Neighbours);

            if (Settings.UseAlignment)
                newDirection += Cohesion(Neighbours);

            if (Settings.UseCohesion)
                newDirection += Alignment(Neighbours);

            newDirection.Normalize();
        }

        ApplyDirection(newDirection);
        ApplyMovement();
    }

    /// <summary>
    /// 
    /// </summary>
    private List<Boid> GetNeighbours()
    {
        var result = new List<Boid>();
        var colliders = Physics.OverlapSphere(transform.position, Settings.ViewRange);

        foreach (var collider in colliders)
        {
            var boid = collider.gameObject.GetComponent<Boid>();
            if (boid != this)
                result.Add(boid);
        }

        return result;
    }




    /// <summary>
    /// 
    /// </summary>
    private Vector3 Seperation(List<Boid> neighbours)
    {
        var result = Vector3.zero;

        foreach (var neighbour in neighbours)
        {
            var difference = transform.position - neighbour.transform.position;
            result += difference.normalized * (1 / difference.magnitude);
        }

        return result * Settings.SeperationWeight;
    }

    /// <summary>
    /// 
    /// </summary>
    private Vector3 Alignment(List<Boid> neighbours)
    {
        var result = Vector3.zero;

        foreach(var neighbour in neighbours)
        {
            var distance = (neighbour.transform.position - transform.position).magnitude;
            result += neighbour.transform.forward * (1 / distance);
        }

        return result.normalized * Settings.AlignmentWeight;
    }
    
    /// <summary>
    /// 
    /// </summary>
    private Vector3 Cohesion(List<Boid> neighbours)
    {
        var result = Vector3.zero;

        foreach(var neighbour in neighbours)
            result += neighbour.transform.position;

        result /= neighbours.Count;
        result = result - transform.position;

        return result.normalized * Settings.CohesionWeight;
    }

    /// <summary>
    /// 
    /// </summary>
    private Vector3 Target()
    {
        var result = Settings.Target - transform.position;
        return result.normalized * Settings.TargetWeight;
    }



    /// <summary>
    /// 
    /// </summary>
    private void ApplyDirection(Vector3 newDirection)
    {
        transform.forward = Vector3.Lerp(
                transform.forward,
                newDirection,
                Settings.DirectionReactionSpeed
        );
    }
    
    /// <summary>
    /// 
    /// </summary>
    private void ApplyMovement()
    {
        transform.position += transform.forward * Settings.MovementSpeed;
    }



    /// <summary>
    /// 
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, Settings.ViewRange);

        Gizmos.color = Color.black;
        foreach(var neighbour in Neighbours)
        {
            Gizmos.DrawRay(transform.position, neighbour.transform.position - transform.position);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Seperation(Neighbours));

        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Alignment(Neighbours));

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Cohesion(Neighbours));
    }
}
