
using System.Collections.Generic;
using _Project.Scripts.Runtime.Character.CharacterStates;
using _Project.Scripts.Runtime.Core.Helpers.Utilities;
using _Project.Scripts.Runtime.Core.Predicates;
using _Project.Scripts.Runtime.Core.StateMachine;
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
        [SerializeField] private float gravityMultiplier = 3f;
        
        [Header("Dash Settings")]
        [SerializeField] private float dashForce = 10f;
        [SerializeField] private float dashDuration = 0.5f;
        [SerializeField] private float dashCooldown = 2f;

        private Transform _mainCam;
        
        
        private float _currentSpeed;
        private float _velocity;
        private float _jumpVelocity;
        private float _dashVelocity = 1f;

        private Vector3 _movement;
        
        
        //Timers

        private List<Timer> _timers;
        private CountdownTimer _jumpTimer;
        private CountdownTimer _jumpCooldownTimer;
        private CountdownTimer _dashTimer;
        private CountdownTimer _dashCooldownTimer;
        
        StateMachine _stateMachine;
        
        // Animator Parameters
        private static readonly int Speed = Animator.StringToHash("Speed");

        #region Unity Methods
        
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
            SetupStateMachine();
            
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
            input.Dash += OnDash;
            //UpdatePublisher.RegisterLateUpdateObserver(this);
        }

        private void OnDisable()
        {
            UpdatePublisher.UnregisterUpdateObserver(this);
            UpdatePublisher.UnregisterFixedUpdateObserver(this);
            input.Jump -= OnJump;
            input.Dash -= OnDash;
            //UpdatePublisher.UnregisterLateUpdateObserver(this);
        }

        public void ObservedUpdate()
        {
            _movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            _stateMachine.Update();
            HandleTimers();
            UpdateAnimator();
        }
        
        public void ObservedFixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }
        
        #endregion

        #region State Machine

        private void SetupStateMachine()
        {
            _stateMachine = new StateMachine();
            DeclareStates();
        }

        private void DeclareStates()
        {
            var locomotionState = new LocomotionState(this, animator);
            var jumpState = new JumpState(this, animator);
            var dashState = new DashState(this, animator);
            
            
            AddTransition(locomotionState, jumpState, new FuncPredicate(()=> _jumpTimer.IsRunning));
            AddTransition(locomotionState, dashState, new FuncPredicate(()=>  _dashTimer.IsRunning));
            AddAnyTransition(locomotionState, new FuncPredicate(() => groundChecker.IsGrounded && !_dashTimer.IsRunning && !_jumpTimer.IsRunning));

            
            _stateMachine.SetState(locomotionState);
        }

        private void AddTransition(IState from, IState to, IPredicate condition) =>
            _stateMachine.AddTransition(from, to, condition);
        
        private void AddAnyTransition(IState to, IPredicate condition) =>
            _stateMachine.AddAnyTransition(to, condition);

        #endregion
        
        #region Timer

        

        
        private void SetupTimers()
        {
            //Setup Timers
            
            _jumpTimer = new CountdownTimer(jumpDuration);
            _jumpCooldownTimer = new CountdownTimer(jumpCooldown);
            _dashTimer = new CountdownTimer(dashDuration);
            _dashCooldownTimer = new CountdownTimer(dashCooldown);
            

            _jumpTimer.OnTimerStart += () => _jumpVelocity = jumpForce;
            _jumpTimer.OnTimerStop += () => _jumpCooldownTimer.Start();
            
            _dashTimer.OnTimerStart += () => _dashVelocity = dashForce;
            _dashTimer.OnTimerStop += () =>
            {
                _dashVelocity = 1f;
                _dashCooldownTimer.Start();
            };
            
            _timers = new List<Timer>(4)
            {
                _jumpTimer,
                _jumpCooldownTimer,
                _dashTimer,
                _dashCooldownTimer
            };
        }
        
        private void HandleTimers()
        {
            foreach (var timer in _timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }
        
        #endregion


        #region Animation

        private void UpdateAnimator()
        {
            animator.SetFloat(Speed, _currentSpeed);
        }
        #endregion

        #region Player Movement


        #region locomotion

        public void HandleMovement()
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
            //Debug.Log($"Dash Velocity: {_dashVelocity}");
            var velocity = adjustedDirection * (moveSpeed * _dashVelocity * Time.fixedDeltaTime);
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


        #endregion

        #region Jump

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
        
        public void HandleJump()
        {
            // if not jumping and grounded,keep jump velocity at 0

            if (!_jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                _jumpVelocity = 0;
                _jumpTimer.Stop();
                return;
            }
            // if jumping or falling calculate velocity

            if (!_jumpTimer.IsRunning)
            {
                //gravity takes over.
                _jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;

            }
            
            // apply velocity
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, _jumpVelocity, rb.linearVelocity.z);
        }

        #endregion


        #region Dash

        private void OnDash(bool performed)
        {
            if (performed && !_dashTimer.IsRunning && !_dashCooldownTimer.IsRunning)
            {
                _dashTimer.Start();
            }
            else if(!performed && _dashTimer.IsRunning)
            {
                _dashTimer.Stop();
            }
        }

        public void HandleDash()
        {
            // if not dashing and grounded,keep dash velocity at 0

            if (!_dashTimer.IsRunning && groundChecker.IsGrounded)
            {
                _dashVelocity = 0;
                _dashTimer.Stop();
                return;
            }

            // apply velocity
            rb.linearVelocity = new Vector3(_dashVelocity, rb.linearVelocity.y, rb.linearVelocity.z);
        }

        #endregion
        

        #endregion
        
    }
}