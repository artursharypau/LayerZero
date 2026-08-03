using System;
using Core.Tick;
using Core.Utils;
using InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.Input
{
    [Serializable]
    public class PlayerInputHandler : IPlayerInput, ITickable, IDisposable
    {
        [SerializeField] private float _jumpBufferDuration = 0.2f;
        [SerializeField] private float _dashBufferDuration = 0.1f;

        private PlayerInputSet _inputSet;
        private PlayerInputSet.PlayerActions _inputActions;
        private BufferedButton _jumpButton;
        private BufferedButton _dashButton;

        public Vector2 Move { get; private set; }

        public void Initialize()
        {
            _inputSet = new PlayerInputSet();
            _inputActions = _inputSet.Player;
            _jumpButton = new BufferedButton(_jumpBufferDuration);
            _dashButton = new BufferedButton(_dashBufferDuration);
        }

        public void Enable()
        {
            _inputSet.Enable();
            _inputActions.Movement.performed += OnMovementPerformed;
            _inputActions.Movement.canceled += OnMovementCanceled;
            _inputActions.Jump.performed += OnJumpPerformed;
            _inputActions.Dash.performed += OnDashPerformed;
        }

        public void Disable()
        {
            _inputSet.Disable();
            _inputActions.Movement.performed -= OnMovementPerformed;
            _inputActions.Movement.canceled -= OnMovementCanceled;
            _inputActions.Jump.performed -= OnJumpPerformed;
            _inputActions.Dash.performed -= OnDashPerformed;
        }

        public void Tick(float deltaTime)
        {
            _jumpButton.Tick(deltaTime);
            _dashButton.Tick(deltaTime);
        }

        public bool WasPerformed(PlayerInputAction action)
        {
            return action switch
            {
                PlayerInputAction.Jump => _jumpButton.IsRequested,
                PlayerInputAction.Dash => _dashButton.IsRequested,
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
                case PlayerInputAction.Dash:
                    _dashButton.Consume();
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

        private void OnDashPerformed(InputAction.CallbackContext context)
        {
            _dashButton.Press();
        }
    }
}
