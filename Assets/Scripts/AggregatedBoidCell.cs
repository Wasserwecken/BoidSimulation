using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AggregatedBoidCell : ICellAggregation<AggregatedBoidCell, Boid>
{
    public int Count { get; set; }
    public Dictionary<int, CellData> BoidTypes;


    /// <summary>
    /// 
    /// </summary>
    public AggregatedBoidCell()
    {
        BoidTypes = new Dictionary<int, CellData>();
        Clear();
    }
    

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public AggregatedBoidCell Add(Boid entity)
    {
        Count++;

        var level = entity.Settings.GetInstanceID();
        if (BoidTypes.ContainsKey(level))
            BoidTypes[level] = BoidTypes[level].Add(entity);
        else
            BoidTypes.Add(level, new CellData().Add(entity));
        
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public AggregatedBoidCell Combine(AggregatedBoidCell other)
    {
        Count += other.Count;

        foreach(var d in other.BoidTypes)
        {
            if (BoidTypes.ContainsKey(d.Key))
                BoidTypes[d.Key] = BoidTypes[d.Key].Combine(d.Value);
            else
                BoidTypes.Add(d.Key, new CellData().Combine(d.Value));
        }

        return this;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public AggregatedBoidCell Finialize()
    {
        foreach (var k in BoidTypes.Keys.ToList())
            BoidTypes[k] = BoidTypes[k].Finalize();

        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public AggregatedBoidCell Clear()
    {
        Count = 0;
        BoidTypes.Clear();

        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    public struct CellData
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
        public CellData Add(Boid b)
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
        public CellData Combine(CellData other)
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
        public CellData Finalize()
        {
            Speed /= Count;
            Position /= Count;
            Direction.Normalize();

            return this;
        }
    }
}
