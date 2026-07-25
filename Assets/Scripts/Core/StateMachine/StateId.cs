namespace Core.StateMachine
{
    public enum StateId
    {
        // Common
        Idle,
        Move,
        Attack,

        // Player
        Dash,
        Jump,
        Fall,
        WallSlide,
        WallJump,
        JumpAttack,

        // Enemy
        Patrol,
        Chase
    }
}
