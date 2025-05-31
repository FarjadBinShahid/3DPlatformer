using _Project.Scripts.Runtime.Environment.Collectibles;
using UnityEngine;

namespace _Project.Scripts.Runtime.Core.SpawnSystem.SpawnFactory
{
    public class EntityFactory<T> : IEntityFactor<T> where T : Entity
    {
        private readonly EntityData[] _data;

        public EntityFactory(EntityData[] data)
        {
            _data = data;
        }

        public T Create(Transform spawnPoint)
        {
            var entityData = _data[Random.Range(0, _data.Length)];
            var instance = GameObject.Instantiate(entityData.prefab, spawnPoint.position, spawnPoint.rotation);
            return instance.GetComponent<T>();
        }
    }
}