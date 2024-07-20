namespace StateMachine;

public interface IStateMachine<T> : IDisposable where T : Enum
{
    public event Action<T> OnStateChanged;
    public IState<T>? CurrentState { get; }
    public IState<T>? PreviousState { get; }
    
    /// <summary>
    /// Should be called to start the state machine.
    /// </summary>
    /// <param name="initialStateType">State type which will be entered after setup.</param>
    public void Run(T initialStateType);
    /// <summary>
    /// Should be called to switch to a new state.
    /// </summary>
    /// <param name="stateType">State type to which the state machine will switch to.</param>
    public void SwitchState(T stateType);
    /// <summary>
    /// Should try to switch to the previous state.
    /// </summary>
    /// <returns>True if successfully switched the state.</returns>
    public bool TrySwitchToPrevious();
}