using Common;

namespace Enemy.States
{
    public class EnemyIdleState : EnemyState
    {
        public EnemyIdleState(int animEntryId, StateMachine fsm, EnemyController controller)
            : base(animEntryId, fsm, controller)
        {
        }
    }
}
