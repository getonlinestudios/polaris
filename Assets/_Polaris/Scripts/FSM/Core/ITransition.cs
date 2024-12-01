namespace Polaris.FSM.Core
{
    public interface ITransition
    {
        IState To { get; } 
        IPredicate Condition { get; }
    }
}