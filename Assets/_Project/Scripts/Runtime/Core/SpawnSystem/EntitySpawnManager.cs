using _Project.Scripts.Runtime.Core.SpawnSystem.SpawnStrategy;
using UnityEngine;

namespace _Project.Scripts.Runtime.Core.SpawnSystem
{
    public abstract class EntitySpawnManager : MonoBehaviour
    {
        protected enum SpawnPointStrategyType
        {
            Linear,
            Random
        }
        [SerializeField] protected SpawnPointStrategyType spawnPointStrategyType = SpawnPointStrategyType.Linear;
        [SerializeField] protected Transform[] spawnPoints;
        
        
        protected ISpawnPointStrategy SpawnPointStrategy;

        protected virtual void Awake()
        {
            SpawnPointStrategy = spawnPointStrategyType switch
            {
                SpawnPointStrategyType.Linear => new LinearSpawnPointStrategy(spawnPoints),
                SpawnPointStrategyType.Random => new RandomSpawnPointStrategy(spawnPoints),
                _ => SpawnPointStrategy
            };
        }

        public abstract void Spawn();

    }
}