namespace StateMachine;

public interface IStateMachine<T> : IDisposable where T : Enum
{
    public event Action<T>? OnStateChanged;
    public IState<T> CurrentState { get; }
    public IState<T>? PreviousState { get; }
    
    public Task SwitchState(T stateType);
}