namespace StateMachine;

public interface IStateMachine<T> : IDisposable where T : Enum
{
    public event Action<T> OnStateChanged;
    public State<T>? CurrentState { get; }
    public State<T>? PreviousState { get; }
    
    public void Run(T initialStateType);
    public void SwitchState(T stateType);
    public bool TrySwitchToPrevious();
}