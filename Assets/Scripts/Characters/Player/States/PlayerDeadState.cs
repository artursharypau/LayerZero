using LayerZero.Characters.Common.Animation;

namespace LayerZero.Characters.Player.States
{
    public class PlayerDeadState : PlayerState
    {
        public PlayerDeadState(PlayerController owner)
            : base(owner, CommonAnimatorParameters.Dead)
        {
        }

        public override int Id => PlayerStateId.Dead;

        public override void Enter()
        {
            base.Enter();

            Input.Disable();
        }
    }
}
