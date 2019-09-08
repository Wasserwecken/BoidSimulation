using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;


public class CellManager<TEntity, TAggregation>
    : ICellManager<TEntity, TAggregation>
    where TEntity : ICellEntity
    where TAggregation : ICellAggregation<TAggregation, TEntity>, new()
{
    public List<TEntity> Entities;
    public ILookup<int, TEntity> EntityBuckets;
    public Dictionary<int, TAggregation> EntityAggregates;
    public float CellScale { get; private set; }
    public float CellSize
    {
        get { return 1 / CellScale; }
        set { CellScale = 1 / value;}
    }
    
    private IEnumerable<TEntity>[] NeighborsResult;
    private TAggregation AggregationResult;


    /// <summary>
    /// 
    /// </summary>
    public CellManager()
    {
        Entities = new List<TEntity>();
        AggregationResult = new TAggregation();
        NeighborsResult = new IEnumerable<TEntity>[8];
    }

    public CellManager(float cellSize)
        : this()
    {
        CellSize = cellSize;
    }
    


    public void Add(TEntity entity)
    {
        if (!Entities.Contains(entity))
        {
            Entities.Add(entity);
            entity.SetCellManager(this);
        }
    }
    
    public void UpdateLists()
    {
        EntityBuckets = Entities.ToLookup(entity => PositionHash(entity.ProvideCellPosition()));
        EntityAggregates = EntityBuckets.ToDictionary(
            bucket => bucket.Key,
            bucket => bucket.Aggregate(new TAggregation(), (TAggregation aggregation, TEntity entity) => aggregation.Add(entity))
        );
    }

    public IEnumerable<TEntity>[] GetNeighbourEntities(Vector3 position)
    {
        var centerOffset = SignVector(position) * new Vector3(0.5f);
        var bucketCenter = CeilVector(position) - centerOffset;
        var relevantDirection = SignVector(position - bucketCenter);

        bucketCenter *= CellScale;

        NeighborsResult[0] = EntityBuckets[Vector3Hash(bucketCenter)];
        NeighborsResult[1] = EntityBuckets[Vector3Hash(bucketCenter + Vector3.One * relevantDirection)];
        NeighborsResult[2] = EntityBuckets[Vector3Hash(bucketCenter + new Vector3(relevantDirection.X, 0f, 0f))];
        NeighborsResult[3] = EntityBuckets[Vector3Hash(bucketCenter + new Vector3(0f, relevantDirection.Y, 0f))];
        NeighborsResult[4] = EntityBuckets[Vector3Hash(bucketCenter + new Vector3(0f, 0f, relevantDirection.Z))];
        NeighborsResult[5] = EntityBuckets[Vector3Hash(bucketCenter + new Vector3(relevantDirection.X, relevantDirection.Y, 0f))];
        NeighborsResult[6] = EntityBuckets[Vector3Hash(bucketCenter + new Vector3(relevantDirection.X, 0f, relevantDirection.Z))];
        NeighborsResult[7] = EntityBuckets[Vector3Hash(bucketCenter + new Vector3(0f, relevantDirection.Y, relevantDirection.Z))];

        return NeighborsResult;
    }
    
    public TAggregation GetNeighborAggregation(Vector3 position) // 7.7ms
    {
        var centerOffset = SignVector(position) * new Vector3(0.5f);
        var bucketCenter = CeilVector(position) - centerOffset;
        var relevantDirection = SignVector(position - bucketCenter);
        
        AggregationResult.Clear().Combine(EntityAggregates[PositionHash(position)]);
        foreach (var center in RelevantNeighborBucketCenters(bucketCenter, relevantDirection))
        {
            var otherBucketHash = Vector3Hash(center);
            if (EntityAggregates.ContainsKey(otherBucketHash))
                AggregationResult.Combine(EntityAggregates[otherBucketHash]);
        }

        return AggregationResult.Finialize();
    }
    
    private Vector3[] RelevantNeighborBucketCenters(Vector3 origin, Vector3 relevantDirection)
    {
        origin *= CellScale;

        return new Vector3[]
        {
            origin + Vector3.One * relevantDirection,

            origin + new Vector3(relevantDirection.X, 0f, 0f),
            origin + new Vector3(0f, relevantDirection.Y, 0f),
            origin + new Vector3(0f, 0f, relevantDirection.Z),

            origin + new Vector3(relevantDirection.X, relevantDirection.Y, 0f),
            origin + new Vector3(relevantDirection.X, 0f, relevantDirection.Z),
            origin + new Vector3(0f, relevantDirection.Y, relevantDirection.Z)
        };
    }
    
    private int PositionHash(Vector3 position)
    {
        return Vector3Hash(Vector3.Multiply(CellScale, position));
    }
    
    private int Vector3Hash(Vector3 vector)
    {
        var hash = 0;

        hash += ((int)vector.X + Math.Sign(vector.X)) * 3;
        hash += ((int)vector.Y + Math.Sign(vector.Y)) * 521527;
        hash += ((int)vector.Z + Math.Sign(vector.Z)) * 1186439;

        return hash;
    }
    
    private Vector3 CeilVector(Vector3 v)
    {
        v.X = (int)v.X + Math.Sign(v.X);
        v.Y = (int)v.Y + Math.Sign(v.Y);
        v.Z = (int)v.Z + Math.Sign(v.Z);

        return v;

        //return new Vector3(
        //    (int)v.X + Math.Sign(v.X),
        //    (int)v.Y + Math.Sign(v.Y),
        //    (int)v.Z + Math.Sign(v.Z)
        //);
    }
    
    private Vector3 SignVector(Vector3 v)
    {
        v.X = Math.Sign(v.X);
        v.Y = Math.Sign(v.Y);
        v.Z = Math.Sign(v.Z);

        return v;

        //return new Vector3(
        //    Math.Sign(v.X),
        //    Math.Sign(v.Y),
        //    Math.Sign(v.Z)
        //);
    }
}
