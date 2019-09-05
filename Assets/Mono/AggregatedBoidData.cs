using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AggregatedBoidData : ICellAggregation<AggregatedBoidData, Boid>
{
    public int Count { get; set; }
    public Vector3 Position;
    public Vector3 Direction;

    

    public AggregatedBoidData()
    {
        Clear();
    }
    


    public AggregatedBoidData Aggregate(Boid entity)
    {
        Count++;

        Position += entity.transform.position;
        Direction += entity.transform.forward;

        return this;
    }

    public AggregatedBoidData Aggregate(AggregatedBoidData other)
    {
        Count += other.Count;
        Position += other.Position;
        Direction += other.Direction;

        return this;
    }
    
    public AggregatedBoidData Finialize()
    {
        Position = Position / Count;
        Direction.Normalize();

        return this;
    }

    public AggregatedBoidData Clear()
    {
        Count = 0;
        Position = Vector3.zero;
        Direction = Vector3.zero;

        return this;
    }
}
