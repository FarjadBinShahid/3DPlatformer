using _Project.Scripts.Runtime.Core.UpdatePublisher;
using UnityEngine;

namespace _Project.Scripts.Runtime.Character
{
    public class GroundChecker : MonoBehaviour, IUpdateObserver
    {
        [SerializeField] private float groundDistance = 0.08f;
        [SerializeField] private LayerMask groundLayer;
        
        public bool IsGrounded {get; private set;}


        private void OnEnable()
        {
            UpdatePublisher.RegisterUpdateObserver(this);
        }

        private void OnDisable()
        {
            UpdatePublisher.UnregisterUpdateObserver(this);
        }

        public void ObservedUpdate()
        {
            IsGrounded = Physics.SphereCast(transform.position, groundDistance, Vector3.down, out _, groundDistance, groundLayer);
        }
    }
}

