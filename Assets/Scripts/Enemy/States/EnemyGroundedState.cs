using Common;

namespace Enemy.States
{
    public abstract class EnemyGroundedState : EnemyState
    {
        protected EnemyGroundedState(int animEntryId, StateMachine fsm, EnemyController controller)
            : base(animEntryId, fsm, controller)
        {
        }

        public override void Update()
        {
            if (Controller.IsPlayerDetected)
            {
                FSM.ChangeState(Controller.BattleState);
            }

            base.Update();
        }
    }
}
