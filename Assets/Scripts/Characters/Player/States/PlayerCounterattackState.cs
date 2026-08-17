using LayerZero.Characters.Common.States;
using LayerZero.Characters.Player.Animation;
using LayerZero.Combat.Attack;
using LayerZero.Core.Timing;

namespace LayerZero.Characters.Player.States
{
    public class PlayerCounterattackState : PlayerState
    {
        private readonly AttackBehaviour _attack;

        private Countdown _timer;

        public PlayerCounterattackState(PlayerController owner)
            : base(owner)
        {
            _attack = new AttackBehaviour(Owner, ResolveAttack);

            On(() => _attack.IsFinished || _timer.IsExpired, ResolveLocomotionState);
        }

        public override int Id => PlayerStateId.Counterattack;

        public override void Enter()
        {
            base.Enter();

            _timer.Start(Config.Counterattack.WindowWaitingDuration);
            _attack.Begin();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Animator.Trigger(PlayerAnimatorParameters.CounterattackTrigger);
        }

        public override void Exit()
        {
            base.Exit();

            _attack.End();
        }

        private AttackDefinition ResolveAttack()
        {
            return Config.Counterattack.Attack;
        }
    }
}
