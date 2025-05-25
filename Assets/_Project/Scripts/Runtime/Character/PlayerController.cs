using System;
using _Project.Scripts.Runtime.Core.UpdatePublisher;
using _Project.Scripts.Runtime.Input;
using KBCore.Refs;
using Unity.Cinemachine;
using UnityEngine;

namespace _Project.Scripts.Runtime.Character
{
    public class PlayerController: ValidatedMonoBehaviour,IUpdateObserver,IFixedUpdateObserver, ILateUpdateObserver
    {
        [Header("Header")] 
        [SerializeField, Self] private CharacterController controller;
        [SerializeField, Self] private Animator animator;
        [SerializeField,Anywhere] CinemachineCamera freeLookCam;
        [SerializeField, Anywhere] private InputReader input;


        [Header("Settings")] 
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float smoothTime = 0.2f;


        private Transform _mainCam;
        
        
        private float _currentSpeed;
        private float _velocity;
        private void Awake()
        {
            if (Camera.main) _mainCam = Camera.main.transform;
            freeLookCam.Follow = transform;
            freeLookCam.LookAt = transform;
            freeLookCam.OnTargetObjectWarped(
                transform,
                transform.position - freeLookCam.transform.position - Vector3.forward);
        }

        private void Start()
        {
            input.EnablePlayerActions();
        }

        private void OnEnable()
        {
            UpdatePublisher.RegisterUpdateObserver(this);
            UpdatePublisher.RegisterFixedUpdateObserver(this);
            UpdatePublisher.RegisterLateUpdateObserver(this);
        }

        private void OnDisable()
        {
            UpdatePublisher.UnregisterUpdateObserver(this);
            UpdatePublisher.UnregisterFixedUpdateObserver(this);
            UpdatePublisher.UnregisterLateUpdateObserver(this);
        }

        public void ObservedUpdate()
        {
            HandleMovement();
        }

        public void ObservedFixedUpdate()
        {
            // noop
        }

        public void ObservedLateUpdate()
        {
            // noop
        }


        private void HandleMovement()
        {
            var movementDirection = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;
            
            //Rotate movement direction to match camera rotation
            var adjustedDirection = Quaternion.AngleAxis(_mainCam.eulerAngles.y, Vector3.up) * movementDirection;

            if (adjustedDirection.magnitude > 0f)
            {
                HandleRotation(adjustedDirection);
                HandleCharacterController(adjustedDirection);
                SmoothSpeed(adjustedDirection.magnitude);
            }
            else
            {
                SmoothSpeed(0f);
            }

        }

        private void HandleRotation(Vector3 adjustedDirection)
        {
            // adjust rotation to match movement direction
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.LookAt(transform.position + adjustedDirection);
        }
        private void HandleCharacterController(Vector3 adjustedDirection)
        {
            //Move the player
            var adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
            controller.Move(adjustedMovement);
        }
        private void SmoothSpeed(float value)
        {
            _currentSpeed = Mathf.SmoothDamp(_currentSpeed, value, ref _velocity, smoothTime);
        }
        
        
        
        
        
    }
}