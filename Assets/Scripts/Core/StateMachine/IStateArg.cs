namespace Core.StateMachine
{
    public interface IStateArg<TArg>
    {
        void Prepare(TArg arg);
    }
}
