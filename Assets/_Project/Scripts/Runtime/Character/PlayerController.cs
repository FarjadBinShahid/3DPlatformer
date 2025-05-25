
using System.Numerics;
using _Project.Scripts.Runtime.Core.UpdatePublisher;
using _Project.Scripts.Runtime.Input;
using KBCore.Refs;
using Unity.Cinemachine;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace _Project.Scripts.Runtime.Character
{
    public class PlayerController: ValidatedMonoBehaviour,IUpdateObserver, IFixedUpdateObserver
    {
        [Header("Header")] 
        [SerializeField, Self] private Rigidbody rb;
        [SerializeField, Self] private Animator animator;
        [SerializeField,Anywhere] private CinemachineCamera freeLookCam;
        [SerializeField, Anywhere] private InputReader input;


        [Header("Settings")] 
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float smoothTime = 0.2f;


        private Transform _mainCam;
        
        
        private float _currentSpeed;
        private float _velocity;

        private Vector3 _movement;
        
        // Animator Parameters
        private static readonly int Speed = Animator.StringToHash("Speed");
        private void Awake()
        {
            if (Camera.main) _mainCam = Camera.main.transform;
            freeLookCam.Follow = transform;
            freeLookCam.LookAt = transform;
            freeLookCam.OnTargetObjectWarped(
                transform,
                transform.position - freeLookCam.transform.position - Vector3.forward);
            
            rb.freezeRotation = true;
        }

        private void Start()
        {
            input.EnablePlayerActions();
        }

        private void OnEnable()
        {
            UpdatePublisher.RegisterUpdateObserver(this);
            UpdatePublisher.RegisterFixedUpdateObserver(this);
            //UpdatePublisher.RegisterLateUpdateObserver(this);
        }

        private void OnDisable()
        {
            UpdatePublisher.UnregisterUpdateObserver(this);
            UpdatePublisher.UnregisterFixedUpdateObserver(this);
            //UpdatePublisher.UnregisterLateUpdateObserver(this);
        }

        public void ObservedUpdate()
        {
            _movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            UpdateAnimator();
        }
        
        public void ObservedFixedUpdate()
        {
            //HandleJump();
            HandleMovement();
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(Speed, _currentSpeed);
        }

        private void HandleMovement()
        {
            //Rotate movement direction to match camera rotation
            var adjustedDirection = Quaternion.AngleAxis(_mainCam.eulerAngles.y, Vector3.up) * _movement;

            if (adjustedDirection.magnitude > 0f)
            {
                HandleRotation(adjustedDirection);
                HandleHorizontalMovement(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(0f);
                
                //Reset horizontal velocity for a snappy stop
                
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }

        }
        
        private void HandleHorizontalMovement(Vector3 adjustedDirection)
        {
            //Move the playervar adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
            var velocity = adjustedDirection * (moveSpeed * Time.fixedDeltaTime);
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            // adjust rotation to match movement direction
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            //transform.LookAt(transform.position + adjustedDirection);
        }
        
        private void SmoothSpeed(float value)
        {
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, value, ref _velocity, smoothTime);
        }


        
    }
}