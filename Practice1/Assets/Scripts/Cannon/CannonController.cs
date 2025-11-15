using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cannon
{
    public class CannonController : MonoBehaviour
    {
        [SerializeField] private float cannonSpeed = 1f;
        [SerializeField] private float cannonRotationSpeed = 5f;
        private CannonInputActions _inputSystem;
        private Vector2 _movementInput;
        private float _rotationInput;

        private void Awake()
        {
            _inputSystem = new CannonInputActions();
        }

        private void OnEnable()
        {
            var ballisticController = GetComponentInChildren<BallisticController>();
            
            _inputSystem.Enable();
            _inputSystem.Control.WASD.performed += OnMovementPerformed;
            _inputSystem.Control.WASD.canceled += OnMovementCanceled;
            _inputSystem.Control.Rotate.performed += OnRotatePerformed;
            _inputSystem.Control.Rotate.canceled += OnRotateCanceled;
            _inputSystem.Control.Shoot.performed += _ => ballisticController.Fire();
        }

        private void OnDisable()
        {
            _inputSystem.Control.WASD.performed -= OnMovementPerformed;
            _inputSystem.Control.WASD.canceled -= OnMovementCanceled;
            _inputSystem.Control.Rotate.performed -= OnRotatePerformed;
            _inputSystem.Control.Rotate.canceled -= OnRotateCanceled;
            _inputSystem.Disable();
        }

        private void Update()
        {
            Movement();
            Rotate();
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            _movementInput = Vector2.zero;
        }

        private void OnRotatePerformed(InputAction.CallbackContext context)
        {
            _rotationInput = context.ReadValue<float>();
        }

        private void OnRotateCanceled(InputAction.CallbackContext context)
        {
            _rotationInput = 0f;
        }

        private void Movement()
        {
            var move = transform.right * _movementInput.x + transform.forward * _movementInput.y;
            transform.position += move * (Time.deltaTime * cannonSpeed);
        }

        private void Rotate()
        {
            transform.eulerAngles += Vector3.up * (_rotationInput * cannonRotationSpeed * Time.deltaTime);
        }
    }
}