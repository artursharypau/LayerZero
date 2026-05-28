using Common;

namespace Enemy
{
    public abstract class EnemyState : StateBase
    {
        protected EnemyController Controller { get; }

        protected EnemyState(int animEntryId, StateMachine fsm, EnemyController controller)
            : base(animEntryId, fsm, controller.Anim)
        {
            Controller = controller;
        }
    }
}
