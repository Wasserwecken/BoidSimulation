using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// https://www.red3d.com/cwr/boids/
/// </summary>
public class Boid : MonoBehaviour
{
    public BoidSettings Settings;
    public List<Boid> OtherBoids;
    public List<Boid> Neighbours;
    


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
        Neighbours = SetNeighbours(OtherBoids);

        var newDirection = (Vector3.zero - transform.position).normalized * Settings.CenterWeight;
        if (Neighbours.Count > 0)
        {
            newDirection += Seperation(Neighbours);
            newDirection += Cohesion(Neighbours);
            newDirection += Alignment(Neighbours);

            newDirection.Normalize();
        }

        ApplyDirection(newDirection);
        ApplyMovement();
    }


    /// <summary>
    /// 
    /// </summary>
    private List<Boid> SetNeighbours(List<Boid> otherBoids)
    {
        var result = new List<Boid>();

        foreach(var boid in OtherBoids)
        {
            if (boid == this)
                continue;

            var distance = (boid.transform.position - transform.position).magnitude;
            if (distance < Settings.ViewRange)
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
