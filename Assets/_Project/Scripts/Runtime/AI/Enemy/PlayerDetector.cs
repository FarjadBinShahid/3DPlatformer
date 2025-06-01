using System;
using _Project.Scripts.Runtime.Core.Helpers.Utilities;
using _Project.Scripts.Runtime.Core.UpdatePublisher;
using UnityEngine;

namespace _Project.Scripts.Runtime.AI.Enemy
{
    public class PlayerDetector : MonoBehaviour, IUpdateObserver
    {
        [SerializeField] private float detectionAngle = 60f; // cone infront of enemy
        [SerializeField] private float detectionRadius = 10f; // large circle around enemy
        [SerializeField] private float innerDetectionRadius = 5f; // small circle around enemy
        [SerializeField] private float detectionCooldown = 1f; // Time between detections
        [SerializeField] private float attackRange = 2f;


        public Transform Player { get; private set; }

        private CountdownTimer _detectionTimer;

        private IDetectionStrategy _detectionStrategy;

        private void Start()
        {

            _detectionTimer = new CountdownTimer(detectionCooldown);
            Player = GameObject.FindGameObjectWithTag("Player").transform;
            _detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
        }

        private void OnEnable()
        {
            UpdatePublisher.RegisterUpdateObserver(this);
        }

        private void OnDisable()
        {
            UpdatePublisher.UnregisterUpdateObserver(this);
        }


        public void ObservedUpdate() => _detectionTimer.Tick(Time.deltaTime);

        public bool CanDetectPlayer() => _detectionTimer.IsRunning || _detectionStrategy.Execute(Player, transform, _detectionTimer);

        public bool CanAttackTarget()
        {
            var directionToPlayer = Player.position - transform.position;
            return directionToPlayer.magnitude <= attackRange;
        }
        public void SetDetectionStrategy(IDetectionStrategy strategy) => _detectionStrategy = strategy;


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, innerDetectionRadius);


            // calculate cone direction
            var forwardConeDirection = Quaternion.Euler(0, detectionAngle / 2f, 0) * transform.forward * detectionRadius;
            var backwardConeDirection = Quaternion.Euler(0, -detectionAngle / 2f, 0) * transform.forward * detectionRadius;

            // Draw Lines to represent the cones
            Gizmos.DrawLine(transform.position, transform.position + forwardConeDirection);
            Gizmos.DrawLine(transform.position, transform.position + backwardConeDirection);
        }
    }
}