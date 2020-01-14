using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PerformanceExample
{
    public class AggregatedBoidChunk : IChunkAggregation<AggregatedBoidChunk, Boid>
    {
        public int Count { get; set; }
        public Dictionary<int, ChunkData> BoidTypes;

        private List<int> TypeIds;


        /// <summary>
        /// 
        /// </summary>
        public AggregatedBoidChunk()
        {
            Count = 0;
            TypeIds = new List<int>();
            BoidTypes = new Dictionary<int, ChunkData>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public AggregatedBoidChunk Include(Boid entity)
        {
            Count++;

            var typeId = entity.Settings.GetInstanceID();

            if (BoidTypes.ContainsKey(typeId))
                BoidTypes[typeId] = BoidTypes[typeId].Add(entity);
            else
            {
                TypeIds.Add(typeId);
                BoidTypes.Add(typeId, new ChunkData().Add(entity));
            }

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

            foreach (var otherType in other.BoidTypes)
            {
                if (BoidTypes.ContainsKey(otherType.Key))
                    BoidTypes[otherType.Key] = BoidTypes[otherType.Key].Combine(otherType.Value);
                else
                {
                    TypeIds.Add(otherType.Key);
                    BoidTypes.Add(otherType.Key, new ChunkData().Combine(otherType.Value));
                }
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AggregatedBoidChunk Finialize()
        {
            foreach (var id in TypeIds)
                BoidTypes[id] = BoidTypes[id].Finalize();

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public AggregatedBoidChunk Clear()
        {
            Count = 0;
            TypeIds.Clear();
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
}