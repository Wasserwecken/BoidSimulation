using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;


namespace PerformanceExample
{
    public class ChunkManager<TEntity, TAggregation>
    : IChunkManager<TEntity, TAggregation>
    where TEntity : IChunkEntity
    where TAggregation : IChunkAggregation<TAggregation, TEntity>, new()
    {
        public List<TEntity> Entities { get; set; }
        public Dictionary<int, List<TEntity>> Chunks;
        public Dictionary<int, TAggregation> ChunkAggregations;
        public float ChunkScale { get; private set; }
        public float ChunkSize
        {
            get { return 1 / ChunkScale; }
            set { ChunkScale = 1 / value; }
        }


        private readonly List<TEntity>[] NeighborsResult;
        private readonly TAggregation AggregationResult;
        private readonly Tuple<List<TEntity>[], TAggregation> NeighborInfoResult;
        private readonly Vector3[] NeighborChunkCenters =
        {
        Vector3.One,
        Vector3.UnitX,
        Vector3.UnitY,
        Vector3.UnitZ,
        new Vector3(1f, 1f, 0f),
        new Vector3(0f, 1f, 1f),
        new Vector3(1f, 0f, 1f)
    };
        private readonly List<TEntity> EmptyChunk;


        /// <summary>
        /// 
        /// </summary>
        public ChunkManager()
        {
            Entities = new List<TEntity>();
            Chunks = new Dictionary<int, List<TEntity>>();
            ChunkAggregations = new Dictionary<int, TAggregation>();

            EmptyChunk = new List<TEntity>();

            AggregationResult = new TAggregation();
            NeighborsResult = new List<TEntity>[NeighborChunkCenters.Length + 1];
            NeighborInfoResult = new Tuple<List<TEntity>[], TAggregation>(NeighborsResult, AggregationResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chunkSize"></param>
        public ChunkManager(float chunkSize)
            : this()
        {
            ChunkSize = chunkSize;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TEntity entity)
        {
            if (!Entities.Contains(entity))
            {
                Entities.Add(entity);
                entity.SetChunkManager(this);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(TEntity entity)
        {
            Entities.Remove(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateChunks()
        {
            Chunks.Clear();
            ChunkAggregations.Clear();


            foreach (var entity in Entities)
            {
                var hash = Vector3Hash(entity.ProvidePosition() * ChunkScale);

                // sort boids into chunks
                if (Chunks.ContainsKey(hash))
                    Chunks[hash].Add(entity);
                else
                    Chunks.Add(hash, new List<TEntity> { entity });

                // build aggragation of the chunks
                if (ChunkAggregations.ContainsKey(hash))
                    ChunkAggregations[hash].Include(entity);
                else
                    ChunkAggregations.Add(hash, new TAggregation().Include(entity));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Tuple<List<TEntity>[], TAggregation> ProvideNeighborInfo(Vector3 position)
        {
            // evaulate the position to get neighbor chunks
            var centerOffset = SignVector(position) * 0.5f;
            var chunkCenter = CeilVector(position) - centerOffset;
            var relevantDirection = SignVector(position - chunkCenter);
            chunkCenter *= ChunkScale;

            // insert the own chunk of the boid to the result
            var hash = Vector3Hash(chunkCenter);
            NeighborsResult[0] = Chunks[hash];
            AggregationResult.Clear().Combine(ChunkAggregations[hash]);

            // add neighbor chunks to the result
            for (int i = 0; i < NeighborChunkCenters.Length; i++)
            {
                var otherHash = Vector3Hash(chunkCenter + (NeighborChunkCenters[i] * relevantDirection));

                NeighborsResult[i + 1] = Chunks.GetValueOr(otherHash, EmptyChunk);
                if (ChunkAggregations.ContainsKey(otherHash))
                    AggregationResult.Combine(ChunkAggregations[otherHash]);
            }
            AggregationResult.Finialize();

            // the result variables holds referenes to the results, there is no need for assignment
            return NeighborInfoResult;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private int Vector3Hash(Vector3 v)
        {
            var hash = 0;

            hash += ((int)v.X + Math.Sign(v.X)) * 3;
            hash += ((int)v.Y + Math.Sign(v.Y)) * 521527;
            hash += ((int)v.Z + Math.Sign(v.Z)) * 1186439;

            return hash;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private Vector3 CeilVector(Vector3 v)
        {
            v.X = (int)v.X + Math.Sign(v.X);
            v.Y = (int)v.Y + Math.Sign(v.Y);
            v.Z = (int)v.Z + Math.Sign(v.Z);

            return v;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private Vector3 SignVector(Vector3 v)
        {
            v.X = Math.Sign(v.X);
            v.Y = Math.Sign(v.Y);
            v.Z = Math.Sign(v.Z);

            return v;
        }
    }
}