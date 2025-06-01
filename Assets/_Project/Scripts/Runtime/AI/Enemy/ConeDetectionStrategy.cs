using _Project.Scripts.Runtime.Core.Helpers.Utilities;
using UnityEngine;

namespace _Project.Scripts.Runtime.AI.Enemy
{
    public class ConeDetectionStrategy : IDetectionStrategy
    {

        private readonly float _detectionAngle;
        private readonly float _detectionRadius;
        private readonly float _innerDetectionRadius;
        public ConeDetectionStrategy(float detectionAngle, float detectionRadius, float innerDetectionRadius)
        {
            _detectionAngle = detectionAngle;
            _detectionRadius = detectionRadius;
            _innerDetectionRadius = innerDetectionRadius;
        }

        public bool Execute(Transform target, Transform detector, CountdownTimer timer)
        {
            if (timer.IsRunning) return false;

            var directionToTarget = target.position - detector.position;
            var angleToTarget = Vector3.Angle(directionToTarget, detector.forward);

            // Check if the target is not within the distance angle + outer radius (aka the cone infront of the detector)
            // or is within the inner radius
            if (!(angleToTarget < _detectionAngle / 2f) || !(directionToTarget.magnitude < _detectionRadius)
                && !(directionToTarget.magnitude < _innerDetectionRadius)) return false;

            timer.Start();
            return true;
        }
    }
}