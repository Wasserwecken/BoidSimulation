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


    void Update()
    {
        var neighbours = GetNeighbours();
        var newDirection = transform.forward;

        if (Settings.UseTarget) newDirection = RuleTarget();

        if (neighbours.Count > 0)
        {
            if (Settings.UseSeperation) newDirection += RuleSeperation(neighbours);
            if (Settings.UseAlignment) newDirection += RuleCohesion(neighbours);
            if (Settings.UseCohesion) newDirection += RuleAlignment(neighbours);

            newDirection.Normalize();
        }

        ApplyDirection(newDirection);
        ApplyMovement();
    }


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


    private Vector3 RuleSeperation(List<Boid> neighbours)
    {
        var result = Vector3.zero;

        foreach (var neighbour in neighbours)
        {
            var difference = transform.position - neighbour.transform.position;
            result += difference.normalized * (1 / difference.magnitude);
        }

        return result * Settings.SeperationWeight;
    }

    private Vector3 RuleAlignment(List<Boid> neighbours)
    {
        var result = Vector3.zero;

        foreach (var neighbour in neighbours)
        {
            var distance = (neighbour.transform.position - transform.position).magnitude;
            result += neighbour.transform.forward * (1 / distance);
        }

        return result * Settings.AlignmentWeight;
    }

    private Vector3 RuleCohesion(List<Boid> neighbours)
    {
        var result = Vector3.zero;

        foreach (var neighbour in neighbours)
            result += neighbour.transform.position;

        result /= neighbours.Count;
        result -= transform.position;

        return result * Settings.CohesionWeight;
    }

    private Vector3 RuleTarget()
    {
        var result = Settings.Target - transform.position;

        return result * Settings.TargetWeight;
    }


    private void ApplyDirection(Vector3 newDirection)
    {
        transform.forward = Vector3.Lerp(
                transform.forward,
                newDirection,
                Settings.DirectionReactionSpeed
        );
    }

    private void ApplyMovement()
    {
        transform.position += transform.forward * Settings.MovementSpeed;
    }


    private void OnDrawGizmosSelected()
    {
        var neighbours = GetNeighbours();

        Gizmos.color = new Color(1, 1, 0f, .05f);
        Gizmos.DrawWireSphere(transform.position, Settings.ViewRange);

        if (neighbours.Count > 0)
        {
            Gizmos.color = new Color(.0f, .0f, .0f, .5f);
            foreach (var neighbour in neighbours)
            {
                Gizmos.DrawRay(transform.position, neighbour.transform.position - transform.position);
            }

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, Settings.UseSeperation ? RuleSeperation(neighbours) * Settings.SeperationWeight : Vector3.zero);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, Settings.UseAlignment ? RuleAlignment(neighbours) * Settings.AlignmentWeight : Vector3.zero);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, Settings.UseCohesion ? RuleCohesion(neighbours) * Settings.CohesionWeight : Vector3.zero);
        }
    }
}
