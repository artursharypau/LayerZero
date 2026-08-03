using InputSystem;
using LayerZero.Characters.Common;
using LayerZero.Characters.Player.Config;
using LayerZero.Core.Timing;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LayerZero.Characters.Player.Input
{
    /// <summary>
    /// Owns the generated input asset and the input buffers, and exposes them as
    /// <see cref="IPlayerInput" />. The only file in the project that knows the Input System exists.
    /// </summary>
    public sealed class PlayerInputModule : CharacterModule, IPlayerInput
    {
        private readonly PlayerInputSettings _settings;
        private readonly BufferedRequest _jump;
        private readonly BufferedRequest _dash;

        private PlayerInputSet _inputSet;
        private PlayerInputSet.PlayerActions _actions;
        private bool _isEnabled;

        public PlayerInputModule(PlayerInputSettings settings)
        {
            _settings = settings;
            _jump = new BufferedRequest(settings.JumpBufferDuration);
            _dash = new BufferedRequest(settings.DashBufferDuration);
        }

        public Vector2 Move { get; private set; }

        protected override void OnInitialize()
        {
            _inputSet = new PlayerInputSet();
            _actions = _inputSet.Player;
        }

        public override void Enable()
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

        public override void Disable()
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

        public override void Tick(float deltaTime)
        {
            _jump.Tick(deltaTime);
            _dash.Tick(deltaTime);
        }

        public override void Dispose()
        {
            Disable();
            _inputSet?.Dispose();
            _inputSet = null;
        }

        public bool WasPerformed(PlayerInputAction action)
        {
            return action switch
            {
                PlayerInputAction.Jump => _jump.IsPending,
                PlayerInputAction.Dash => _dash.IsPending,
                PlayerInputAction.Attack => _isEnabled && _actions.Attack.WasPerformedThisFrame(),
                _ => false
            };
        }

        public void Consume(PlayerInputAction action)
        {
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
