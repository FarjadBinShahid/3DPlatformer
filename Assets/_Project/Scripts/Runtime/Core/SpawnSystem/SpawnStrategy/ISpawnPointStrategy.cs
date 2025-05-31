using UnityEngine;

namespace _Project.Scripts.Runtime.Core.SpawnSystem.SpawnStrategy
{
    public interface ISpawnPointStrategy
    {
        Transform NextSpawnPoint();
    }
}