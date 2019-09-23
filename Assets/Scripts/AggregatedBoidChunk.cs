using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AggregatedBoidChunk : IChunkAggregation<AggregatedBoidChunk, Boid>
{
    public int Count { get; set; }
    public Dictionary<int, ChunkData> BoidTypes;


    /// <summary>
    /// 
    /// </summary>
    public AggregatedBoidChunk()
    {
        BoidTypes = new Dictionary<int, ChunkData>();
        Clear();
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public AggregatedBoidChunk Include(Boid entity)
    {
        Count++;

        var level = entity.Settings.GetInstanceID();
        if (BoidTypes.ContainsKey(level))
            BoidTypes[level] = BoidTypes[level].Add(entity);
        else
            BoidTypes.Add(level, new ChunkData().Add(entity));
        
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public AggregatedBoidChunk Combine(AggregatedBoidChunk other)
    {
        Count += other.Count;

        foreach(var d in other.BoidTypes)
        {
            if (BoidTypes.ContainsKey(d.Key))
                BoidTypes[d.Key] = BoidTypes[d.Key].Combine(d.Value);
            else
                BoidTypes.Add(d.Key, new ChunkData().Combine(d.Value));
        }

        return this;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public AggregatedBoidChunk Finialize()
    {
        foreach (var k in BoidTypes.Keys.ToList())
            BoidTypes[k] = BoidTypes[k].Finalize();

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public AggregatedBoidChunk Clear()
    {
        Count = 0;
        BoidTypes.Clear();

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    public struct ChunkData
    {
        public int Count { get; set; }
        public Vector3 Position;
        public Vector3 Direction;
        public float Speed;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public ChunkData Add(Boid b)
        {
            Count++;

            Position += b.transform.position;
            Direction += b.transform.forward;
            Speed += b.CurrentSpeed;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public ChunkData Combine(ChunkData other)
        {
            Count += other.Count;
            Position += other.Position;
            Direction += other.Direction;
            Speed += other.Speed;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ChunkData Finalize()
        {
            Speed /= Count;
            Position /= Count;
            Direction.Normalize();

            return this;
        }
    }
}
