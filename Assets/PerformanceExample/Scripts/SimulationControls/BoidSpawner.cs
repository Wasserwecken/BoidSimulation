using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PerformanceExample
{
    public class BoidSpawner : MonoBehaviour
    {
        public List<SpawnEntry> SpawnEntries;
        public BoidChunkManager ResponsibleManager;
        public float AreaRadius = 10;


        /// <summary>
        /// 
        /// </summary>
        void Start()
        {
            foreach (var entry in SpawnEntries)
                entry.Count.Times(() => Spawn(entry.Prefab));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefab"></param>
        public void Spawn(Boid prefab)
        {
            var startPosition = Random.insideUnitSphere * AreaRadius + transform.position;
            Boid boid = Instantiate(
                    prefab,
                    startPosition,
                    Random.rotation,
                    ResponsibleManager.transform
            );
            ResponsibleManager.Add(boid);
        }


        /// <summary>
        /// 
        /// </summary>
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, AreaRadius);
        }


        /// <summary>
        /// 
        /// </summary>
        [System.Serializable]
        public struct SpawnEntry
        {
            public Boid Prefab;
            public int Count;
        }
    }
}