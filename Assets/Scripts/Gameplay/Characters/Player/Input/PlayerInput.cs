using System;
using LayerZero.Core.Timing;
using LayerZero.Gameplay.Characters.Player.Config;
using LayerZero.Gameplay.Combat.Elements;
using LayerZero.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LayerZero.Gameplay.Characters.Player.Input
{
    internal sealed class PlayerInput : IPlayerInput, IDisposable
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

        public event Action<ElementKind> ElementSelected;

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
            _actions.SelectFireElement.performed += OnSelectFireElementPerformed;
            _actions.SelectIceElement.performed += OnSelectIceElementPerformed;
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
            _actions.SelectFireElement.performed -= OnSelectFireElementPerformed;
            _actions.SelectIceElement.performed -= OnSelectIceElementPerformed;

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
                PlayerInputAction.Attack => _actions.Attack.WasPerformedThisFrame(),
                PlayerInputAction.Counterattack => _actions.Counterattack.WasPerformedThisFrame(),
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

        private void OnSelectFireElementPerformed(InputAction.CallbackContext context)
        {
            ElementSelected?.Invoke(ElementKind.Fire);
        }

        private void OnSelectIceElementPerformed(InputAction.CallbackContext context)
        {
            ElementSelected?.Invoke(ElementKind.Ice);
        }
    }
}
