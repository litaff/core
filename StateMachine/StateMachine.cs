namespace StateMachine;

using Logger;

[Serializable]
public abstract class StateMachine<T> : IStateMachine<T> where T : Enum
{
    protected readonly Dictionary<T, IState<T>> States;
    protected ILogger Logger { get; private set; }
    
    public IState<T>? CurrentState { get; protected set; }
    public IState<T>? PreviousState { get; protected set; }
    
    public event Action<T>? OnStateChanged;

    /// <summary>
    /// Creates a new state machine without states. <see cref="RegisterStates"/>
    /// needs to be called, before the state machine starts working.
    /// </summary>
    public StateMachine(ILogger logger)
    {
        Logger = logger;
        States = new Dictionary<T, IState<T>>();
    }
    
    /// <summary>
    /// Creates a new state machine with the given states.
    /// </summary>
    public StateMachine(ILogger logger, params IState<T>[] states)
    {
        Logger = logger;
        States = new Dictionary<T, IState<T>>();
        RegisterStates(states);
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
            Logger.Log($"[StateMachine] Exiting state: {CurrentState.StateType}.");
            CurrentState.OnExit();
        }
        
        if (!States.TryGetValue(stateType, out var value))
        {
            throw new ArgumentException($"State {stateType} does not exist in the state machine");
        }
        
        PreviousState = CurrentState;
        CurrentState = value;
        
        Logger.Log($"[StateMachine] Entering state: {CurrentState.StateType}.");
        CurrentState.OnEnter();
        
        OnStateChanged?.Invoke(stateType);
        Logger.Log($"[StateMachine] Switched state to: {CurrentState.StateType}.");
    }

    /// <summary>
    /// Tries to switch to the previous state.
    /// </summary>
    /// <returns>True if switch was successful.</returns>
    public bool TrySwitchToPrevious()
    {
        if (PreviousState == null)
        {
            Logger.Log("[StateMachine] No previous state to switch to.");
            return false;
        }
        
        SwitchState(PreviousState.StateType);
        return true;
    }

    public void RegisterStates(params IState<T>[] states)
    {
        foreach (var state in states)
        {
            RegisterState(state);
        }
    }
    
    /// <summary>
    /// Clears the states and calls the OnExit of the current state.
    /// </summary>
    public void Dispose()
    {
        States.Clear();
        CurrentState?.OnExit();
        CurrentState = null;
        Logger.Log("[StateMachine] Disposed.");
    }

    /// <summary>
    /// Registers the state to the state machine.
    /// If the state type already exists in the state machine, an ArgumentException is thrown.
    /// </summary>
    private void RegisterState(IState<T> state)
    {
        state.StateMachine = this;
        if (States.TryAdd(state.StateType, state))
        {
            Logger.Log($"[StateMachine] State registered: {state.StateType}.");
            return;
        }
        throw new ArgumentException($"State {state.StateType} already exists in the state machine");
    }
}