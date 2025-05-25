using System;
using System.Collections;
using _Project.Scripts.Runtime.Input;
using KBCore.Refs;
using Unity.Cinemachine;
using UnityEngine;

namespace _Project.Scripts.Runtime.Core.Camera
{
    public class CameraManager: ValidatedMonoBehaviour
    {
        [Header("Reference")]
        [SerializeField, Anywhere] private InputReader input;
        [SerializeField, Anywhere] private CinemachineCamera freeLookCam;
        
        [Header("Settings")]
        [SerializeField, Range(0.5f, 3f)] float speedMultiplier= 1f;

        private bool _isRmbPressed;
        private bool _isDeviceMouse;
        private bool _cameraMovementLock;

        private void OnEnable()
        {
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        private void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
        {
            if (_cameraMovementLock) return;

            if (isDeviceMouse && !_isRmbPressed) return;
            
            // if the device is mouse use fixedDeltaTime, otherwise use delta time.
            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;
            
            // set camera axis values
            
            
        }

        private void OnDisableMouseControlCamera()
        {
            _isRmbPressed = false;
            
            //unlock the cursor and make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            //Reset the camera axis to prevent jumping when re-enabling mouse
            
        }

        private void OnEnableMouseControlCamera()
        {
            _isRmbPressed = true;
            
            //Lock cursor to the center ogf the screen and hide it
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(DisableMouseForFrame());
        }

        private IEnumerator DisableMouseForFrame()
        {
            _cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            _cameraMovementLock = false;
        }


        private void OnDisable()
        {
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }
    }
}