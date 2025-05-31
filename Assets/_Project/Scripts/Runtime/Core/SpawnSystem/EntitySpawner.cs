using _Project.Scripts.Runtime.Core.SpawnSystem.SpawnFactory;
using _Project.Scripts.Runtime.Core.SpawnSystem.SpawnStrategy;
using _Project.Scripts.Runtime.Environment.Collectibles;

namespace _Project.Scripts.Runtime.Core.SpawnSystem
{
    public class EntitySpawner<T> where T : Entity
    {
        private readonly IEntityFactor<T> _entityFactor;
        private readonly ISpawnPointStrategy _spawnPointStrategy;

        public EntitySpawner(IEntityFactor<T> entityFactor, ISpawnPointStrategy spawnPointStrategy)
        {
            _entityFactor = entityFactor;
            _spawnPointStrategy = spawnPointStrategy;
        }

        public T Spawn()
        {
            return _entityFactor.Create(_spawnPointStrategy.NextSpawnPoint());
        }
    }
}