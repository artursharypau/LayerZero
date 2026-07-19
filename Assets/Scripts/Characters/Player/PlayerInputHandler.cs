using System;
using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player
{
    public class PlayerInputHandler : IDisposable
    {
        private readonly PlayerInputSet _inputSet;

        private PlayerInputSet.PlayerActions _inputActions;

        public Vector2 MoveInput { get; private set; }
        public bool JumpRequested { get; private set; }

        public PlayerInputHandler()
        {
            _inputSet = new PlayerInputSet();
            _inputActions = _inputSet.Player;

            _inputActions.Enable();
            _inputActions.Movement.performed += OnMovementPerformed;
            _inputActions.Movement.canceled += OnMovementCanceled;
            _inputActions.Jump.performed += OnJumpPerformed;
        }

        public void Dispose()
        {
            _inputActions.Disable();
            _inputActions.Movement.performed -= OnMovementPerformed;
            _inputActions.Movement.canceled -= OnMovementCanceled;
            _inputActions.Jump.performed -= OnJumpPerformed;

            _inputSet.Dispose();
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            MoveInput = Vector2.zero;
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            JumpRequested = true;
        }
    }
}
