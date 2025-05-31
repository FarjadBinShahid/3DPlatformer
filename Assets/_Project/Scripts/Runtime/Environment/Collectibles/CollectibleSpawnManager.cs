using System;
using _Project.Scripts.Runtime.Core.Helpers.Utilities;
using _Project.Scripts.Runtime.Core.SpawnSystem;
using _Project.Scripts.Runtime.Core.SpawnSystem.SpawnFactory;
using _Project.Scripts.Runtime.Core.UpdatePublisher;
using UnityEngine;

namespace _Project.Scripts.Runtime.Environment.Collectibles
{
    public class CollectibleSpawnManager : EntitySpawnManager, IUpdateObserver
    {
        [SerializeField] private CollectibleData[] collectibleData;
        [SerializeField] private float spawnInterval = 1f;


        private EntitySpawner<Collectible> _spawner;

        private CountdownTimer _spawnTimer;
        private int _counter;
        
        protected override void Awake()
        {
            base.Awake();

            _spawner = new EntitySpawner<Collectible>(
                new EntityFactory<Collectible>(collectibleData),
                SpawnPointStrategy);
            
            SetupTimers();
        }

        private void Start() => _spawnTimer.Start();

        private void OnEnable()
        {
            UpdatePublisher.RegisterUpdateObserver(this);
        }

        private void OnDisable()
        {
            UpdatePublisher.UnregisterUpdateObserver(this);
        }

        void IUpdateObserver.ObservedUpdate() => _spawnTimer.Tick(Time.deltaTime);

        public override void Spawn() => _spawner.Spawn();


        private void SetupTimers()
        {
            _spawnTimer = new CountdownTimer(spawnInterval);
            _spawnTimer.OnTimerStop += OnTimerStop;
        }

        private void OnTimerStop()
        {
            if (_counter++ >= spawnPoints.Length)
            {
                _spawnTimer.Stop();
                return;
            }
            Spawn();
            _spawnTimer.Start();
        }

        
    }
}