namespace StateMachine;

public abstract class StateMachine<T> : IStateMachine<T> where T : Enum
{
    protected readonly Dictionary<T, State<T>> States;
    
    public State<T>? CurrentState { get; protected set; }
    public State<T>? PreviousState { get; protected set; }
    
    public event Action<T>? OnStateChanged;

    /// <summary>
    /// Creates a new state machine with the given states.
    /// </summary>
    public StateMachine(params State<T>[] states)
    {
        States = new Dictionary<T, State<T>>();
        
        foreach (var state in states)
        {
            RegisterState(state);
        }
    }

    /// <summary>
    /// Starts the state machine. Switches to the <see cref="initialStateType"/>.
    /// </summary>
    public void Run(T initialStateType)
    {
        SwitchState(initialStateType);
    }

    /// <summary>
    /// Switches to the given state type. If the state type is the same as the current state type, nothing happens.
    /// If the state type does not exist in the state machine, an ArgumentException is thrown.
    /// Current state's OnExit is called before switching to the new state.
    /// New state's OnEnter is called after switching to it.
    /// <see cref="OnStateChanged"/> event is invoked after switching to the new state.
    /// </summary>
    public void SwitchState(T stateType)
    {
        if (CurrentState != null)
        {
            if(CurrentState.StateType.Equals(stateType))
            {
                return;
            }
            CurrentState.OnExit();
        }
        
        if (!States.TryGetValue(stateType, out var value))
        {
            throw new ArgumentException($"State {stateType} does not exist in the state machine");
        }
        
        PreviousState = CurrentState;
        CurrentState = value;
        
        CurrentState.OnEnter();
        
        OnStateChanged?.Invoke(stateType);
    }

    /// <summary>
    /// Tries to switch to the previous state.
    /// </summary>
    /// <returns>True if switch was successful.</returns>
    public bool TrySwitchToPrevious()
    {
        if (PreviousState == null) return false;
        
        SwitchState(PreviousState.StateType);
        return true;
    }
    
    /// <summary>
    /// Clears the states and calls the OnExit of the current state.
    /// </summary>
    public void Dispose()
    {
        States.Clear();
        CurrentState?.OnExit();
        CurrentState = null;
    }

    /// <summary>
    /// Registers the state to the state machine.
    /// If the state type already exists in the state machine, an ArgumentException is thrown.
    /// </summary>
    private void RegisterState(State<T> state)
    {
        state.StateMachine = this;
        if(States.TryAdd(state.StateType, state)) return;
        throw new ArgumentException($"State {state.StateType} already exists in the state machine");
    }
}