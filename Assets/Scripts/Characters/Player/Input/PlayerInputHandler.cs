using System;
using Infrastructure.Tick;
using Infrastructure.Utils;
using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.Input
{
    [Serializable]
    public class PlayerInputHandler : IPlayerInput, ITickable, IDisposable
    {
        [SerializeField] private float _jumpBufferDuration = 0.2f;

        private PlayerInputSet _inputSet;
        private PlayerInputSet.PlayerActions _inputActions;
        private BufferedButton _jumpButton;

        public Vector2 Move { get; private set; }

        public void Initialize()
        {
            _inputSet = new PlayerInputSet();
            _inputActions = _inputSet.Player;
            _jumpButton = new BufferedButton(_jumpBufferDuration);
        }

        public void Enable()
        {
            _inputSet.Enable();
            _inputActions.Movement.performed += OnMovementPerformed;
            _inputActions.Movement.canceled += OnMovementCanceled;
            _inputActions.Jump.performed += OnJumpPerformed;
        }

        public void Disable()
        {
            _inputSet.Disable();
            _inputActions.Movement.performed -= OnMovementPerformed;
            _inputActions.Movement.canceled -= OnMovementCanceled;
            _inputActions.Jump.performed -= OnJumpPerformed;
        }

        public void Tick(float deltaTime)
        {
            _jumpButton.Tick(deltaTime);
        }

        public bool WasPerformed(PlayerInputAction action)
        {
            return action switch
            {
                PlayerInputAction.Jump => _jumpButton.IsRequested,
                PlayerInputAction.Dash => _inputActions.Dash.WasPerformedThisFrame(),
                PlayerInputAction.Attack => _inputActions.Attack.WasPerformedThisFrame(),
                _ => false
            };
        }

        public void Consume(PlayerInputAction action)
        {
            switch (action)
            {
                case PlayerInputAction.Jump:
                    _jumpButton.Consume();
                    break;
            }
        }

        public void Dispose()
        {
            Disable();
            _inputSet.Dispose();
        }

        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        private void OnMovementCanceled(InputAction.CallbackContext context)
        {
            Move = Vector2.zero;
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            _jumpButton.Press();
        }
    }
}
