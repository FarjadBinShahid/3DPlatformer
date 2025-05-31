using _Project.Scripts.Runtime.Environment.Collectibles;
using UnityEngine;

namespace _Project.Scripts.Runtime.Core.SpawnSystem.SpawnFactory
{
    public interface IEntityFactor<T> where T : Entity
    {
        T Create(Transform spawnPoint);    
    }
    
}