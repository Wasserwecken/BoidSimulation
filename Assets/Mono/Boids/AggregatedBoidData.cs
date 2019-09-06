using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AggregatedBoidData : ICellAggregation<AggregatedBoidData, Boid>
{
    public int Count { get; set; }

    public struct BoidData
    {
        public int Count { get; set; }
        public Vector3 Position;
        public Vector3 Direction;


        public BoidData Add(Boid b)
        {
            Count++;

            Position += b.transform.position;
            Direction += b.transform.forward;

            return this;
        }

        public BoidData Combine(BoidData other)
        {
            Count += other.Count;
            Position += other.Position;
            Direction += other.Direction;

            return this;
        }

        public BoidData Finalize()
        {
            Position = Position / Count;
            Direction.Normalize();

            return this;
        }
    }

    public Dictionary<int, BoidData> Data;


    

    public AggregatedBoidData()
    {
        Data = new Dictionary<int, BoidData>();
        Clear();
    }
    


    public AggregatedBoidData Aggregate(Boid entity)
    {
        Count++;

        var level = entity.Settings.GetInstanceID();
        if (Data.ContainsKey(level))
            Data[level] = Data[level].Add(entity);
        else
            Data.Add(level, new BoidData().Add(entity));
        
        return this;
    }

    public AggregatedBoidData Aggregate(AggregatedBoidData other)
    {
        Count += other.Count;

        foreach(var d in other.Data)
        {
            if (Data.ContainsKey(d.Key))
                Data[d.Key] = Data[d.Key].Combine(d.Value);
            else
                Data.Add(d.Key, new BoidData().Combine(d.Value));
        }

        return this;
    }
    
    public AggregatedBoidData Finialize()
    {
        foreach (var k in Data.Keys.ToList())
            Data[k] = Data[k].Finalize();

        return this;
    }

    public AggregatedBoidData Clear()
    {
        Count = 0;
        Data.Clear();

        return this;
    }
}
