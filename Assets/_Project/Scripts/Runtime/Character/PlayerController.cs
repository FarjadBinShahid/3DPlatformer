
using System.Collections.Generic;
using _Project.Scripts.Runtime.Core.Helpers.Utilities;
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
        [SerializeField, Self] private GroundChecker groundChecker;
        [SerializeField, Self] private Animator animator;
        [SerializeField,Anywhere] private CinemachineCamera freeLookCam;
        [SerializeField, Anywhere] private InputReader input;


        [Header("Movement Settings")] 
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float smoothTime = 0.2f;

        
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float jumpDuration = 0.5f;
        [SerializeField] private float jumpCooldown = 0f;
        [SerializeField] private float jumpMaxHeight = 2f;
        [SerializeField] private float gravityMultiplier = 3f;
        

        private Transform _mainCam;
        
        
        private float _currentSpeed;
        private float _velocity;
        private float _jumpVelocity;

        private Vector3 _movement;
        
        
        //Timers

        private List<Timer> _timers;
        private CountdownTimer _jumpTimer;
        private CountdownTimer _jumpCooldownTimer;
        
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

            SetupTimers();
        }

        private void Start()
        {
            input.EnablePlayerActions();
        }

        private void OnEnable()
        {
            UpdatePublisher.RegisterUpdateObserver(this);
            UpdatePublisher.RegisterFixedUpdateObserver(this);

            input.Jump += OnJump;
            //UpdatePublisher.RegisterLateUpdateObserver(this);
        }

        private void OnDisable()
        {
            UpdatePublisher.UnregisterUpdateObserver(this);
            UpdatePublisher.UnregisterFixedUpdateObserver(this);
            input.Jump -= OnJump;
            //UpdatePublisher.UnregisterLateUpdateObserver(this);
        }

        public void ObservedUpdate()
        {
            _movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            HandleTimers();
            UpdateAnimator();
        }
        
        public void ObservedFixedUpdate()
        {
            HandleJump();
            HandleMovement();
        }

        private void SetupTimers()
        {
            //Setup Timers
            
            _jumpTimer = new CountdownTimer(jumpDuration);
            _jumpCooldownTimer = new CountdownTimer(jumpCooldown);
            _timers = new List<Timer>(2)
            {
                _jumpTimer,
                _jumpCooldownTimer
            };

            _jumpTimer.OnTimerStop += () => _jumpCooldownTimer.Start();
        }
        
        private void HandleTimers()
        {
            foreach (var timer in _timers)
            {
                timer.Tick(Time.deltaTime);
            }
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
        
        private void OnJump(bool performed)
        {
             if (performed && !_jumpTimer.IsRunning && !_jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
            {
                _jumpTimer.Start();
            }
            else if(!performed && _jumpTimer.IsRunning)
            {
                _jumpTimer.Stop();
            }
        }
        
        private void HandleJump()
        {
            // if not jumping and grounded,keep jump velocity at 0

            if (!_jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                _jumpVelocity = 0;
                _jumpTimer.Stop();
                return;
            }
            // if jumping or falling calculate velocity

            if (_jumpTimer.IsRunning)
            {
                // progress point for initial burst of velocity
                var launchPoint = 0.9f;
                if (_jumpTimer.Progress > launchPoint)
                {
                    // calculate the celocity requikred to reach the jump height using physics equation v = sqrt(2gh)
                    _jumpVelocity = Mathf.Sqrt(2 * jumpMaxHeight * Mathf.Abs(Physics.gravity.y));
                }
                else
                {
                    // gravity apply less velocity on the jump progress.
                    _jumpVelocity += (1 - _jumpTimer.Progress) * jumpForce * Time.fixedDeltaTime;
                }
            }
            else 
            {
                //gravity takes over.
                _jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;

            }
            
            // apply velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, _jumpVelocity, rb.linearVelocity.z);
        }


        
    }
}