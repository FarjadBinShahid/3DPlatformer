using _Project.Scripts.Runtime.Core.Predicates;

namespace _Project.Scripts.Runtime.Core.StateMachine.Transition
{
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}