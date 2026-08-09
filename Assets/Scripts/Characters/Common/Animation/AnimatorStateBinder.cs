using LayerZero.Core.StateMachine;

namespace LayerZero.Characters.Common.Animation
{
    public sealed class AnimatorStateBinder
    {
        private readonly CharacterAnimator _animator;
        private readonly StateMachine _stateMachine;

        public AnimatorStateBinder(CharacterAnimator animator, StateMachine stateMachine)
        {
            _animator = animator;
            _stateMachine = stateMachine;
        }

        public void Bind()
        {
            _stateMachine.StateEntered += Apply;

            if (_stateMachine.Current != null)
            {
                Apply(_stateMachine.Current);
            }
        }

        public void Unbind()
        {
            _stateMachine.StateEntered -= Apply;
        }

        private void Apply(StateBase state)
        {
            _animator.SetInt(CommonAnimatorParameters.State, state.Id);
        }
    }
}
