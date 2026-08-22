using System;
using InputSystem;
using LayerZero.Core.Timing;
using LayerZero.Gameplay.Characters.Player.Config;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LayerZero.Gameplay.Characters.Player.Input
{
    public sealed class PlayerInput : IPlayerInput, IDisposable
    {
        private readonly BufferedRequest _jump;
        private readonly BufferedRequest _dash;

        private PlayerInputSet _inputSet;
        private PlayerInputSet.PlayerActions _actions;
        private bool _isEnabled;

        public PlayerInput(PlayerInputConfig config)
        {
            _jump = new BufferedRequest(config.JumpBufferDuration);
            _dash = new BufferedRequest(config.DashBufferDuration);

            _inputSet = new PlayerInputSet();
            _actions = _inputSet.Player;
        }

        public Vector2 Move { get; private set; }

        public void Enable()
        {
            if (_isEnabled)
            {
                return;
            }

            _isEnabled = true;
            _inputSet.Enable();

            _actions.Movement.performed += OnMovePerformed;
            _actions.Movement.canceled += OnMoveCanceled;
            _actions.Jump.performed += OnJumpPerformed;
            _actions.Dash.performed += OnDashPerformed;
        }

        public void Disable()
        {
            if (!_isEnabled)
            {
                return;
            }

            _isEnabled = false;

            _actions.Movement.performed -= OnMovePerformed;
            _actions.Movement.canceled -= OnMoveCanceled;
            _actions.Jump.performed -= OnJumpPerformed;
            _actions.Dash.performed -= OnDashPerformed;

            _inputSet.Disable();
            Move = Vector2.zero;
        }

        public void Dispose()
        {
            Disable();
            _inputSet?.Dispose();
            _inputSet = null;
        }

        public bool WasPerformed(PlayerInputAction action)
        {
            if (!_isEnabled)
            {
                return false;
            }

            return action switch
            {
                PlayerInputAction.Jump => _jump.IsPending,
                PlayerInputAction.Dash => _dash.IsPending,
                PlayerInputAction.Attack => _isEnabled && _actions.Attack.WasPerformedThisFrame(),
                PlayerInputAction.Counterattack => _isEnabled && _actions.Counterattack.WasPerformedThisFrame(),
                _ => false
            };
        }

        public void Consume(PlayerInputAction action)
        {
            if (!_isEnabled)
            {
                return;
            }

            switch (action)
            {
                case PlayerInputAction.Jump:
                    _jump.Consume();
                    break;
                case PlayerInputAction.Dash:
                    _dash.Consume();
                    break;
            }
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            Move = Vector2.zero;
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            _jump.Raise();
        }

        private void OnDashPerformed(InputAction.CallbackContext context)
        {
            _dash.Raise();
        }
    }
}
