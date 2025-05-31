namespace _Project.Scripts.Runtime.Core.StateMachine
{
    public interface IState 
    {
        void OnEnter();
        void OnExit();
        void Update();
        void FixedUpdate();
    }
}
