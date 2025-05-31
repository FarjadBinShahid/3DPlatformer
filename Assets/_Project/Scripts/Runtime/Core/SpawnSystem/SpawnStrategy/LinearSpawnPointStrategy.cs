using UnityEngine;

namespace _Project.Scripts.Runtime.Core.SpawnSystem.SpawnStrategy
{
    public class LinearSpawnPointStrategy : ISpawnPointStrategy
    {
        private int _index = 0;
        private Transform[] _spawnPoints;

        public LinearSpawnPointStrategy(Transform[] spawnPoints)
        {
            _spawnPoints = spawnPoints;
        }
        public Transform NextSpawnPoint()
        {
            var result = _spawnPoints[_index];
            _index = (_index +1) % _spawnPoints.Length;
            return result;
        }
    }
}