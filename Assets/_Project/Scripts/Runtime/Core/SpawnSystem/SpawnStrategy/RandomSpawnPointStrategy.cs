using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Runtime.Core.SpawnSystem.SpawnStrategy
{
    public class RandomSpawnPointStrategy : ISpawnPointStrategy
    {
        private List<Transform> _unUsedSpawnPoints;
        private Transform[] _spawnPoints;


        public RandomSpawnPointStrategy(Transform[] spawnPoints)
        {
            this._spawnPoints = spawnPoints;
            _unUsedSpawnPoints = new List<Transform>(spawnPoints);
        }
        public Transform NextSpawnPoint()
        {
            if (!_unUsedSpawnPoints.Any())
            {
                _unUsedSpawnPoints = new List<Transform>(_spawnPoints);
            }
            
            var randomIndex = Random.Range(0, _unUsedSpawnPoints.Count);
            var result = _unUsedSpawnPoints[randomIndex];
            _unUsedSpawnPoints.RemoveAt(randomIndex);
            return result;
        }
    }
}