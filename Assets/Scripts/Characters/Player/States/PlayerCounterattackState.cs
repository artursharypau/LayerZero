using LayerZero.Characters.Common.States;
using LayerZero.Characters.Player.Animation;
using LayerZero.Core.Timing;

namespace LayerZero.Characters.Player.States
{
    public sealed class PlayerCounterattackState : PlayerState
    {
        private readonly AttackBehaviour _attack;

        private bool _isParried;
        private Countdown _recoveryTimer;

        public PlayerCounterattackState(PlayerController owner)
            : base(owner)
        {
            _attack = new AttackBehaviour(owner, () => Config.Counterattack.Attack);

            On(() => _isParried ? _attack.IsFinished : _recoveryTimer.IsExpired, ResolveLocomotionState);
        }

        public override int Id => PlayerStateId.Counterattack;

        public override void Enter()
        {
            base.Enter();

            _isParried = false;
            _recoveryTimer.Start(Config.Counterattack.RecoveryDuration);

            Movement.SetVelocityX(0f);
            TryParry();
        }

        public override void Update()
        {
            base.Update();

            if (!_isParried)
            {
                TryParry();
            }
        }

        public override void Exit()
        {
            base.Exit();

            _attack.End();
        }

        private void TryParry()
        {
            if (!Owner.Combat.TryParry())
            {
                return;
            }

            _isParried = true;

            _attack.Begin();
            Animator.Trigger(PlayerAnimatorParameters.CounterattackTrigger);
        }
    }
}
