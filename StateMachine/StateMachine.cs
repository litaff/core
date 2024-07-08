namespace StateMachine;

public abstract class StateMachine<T> : IStateMachine<T> where T : Enum
{
    protected readonly Dictionary<T, State<T>> States;
    
    public State<T>? CurrentState { get; protected set; }
    public State<T>? PreviousState { get; protected set; }
    
    public event Action<T>? OnStateChanged;

    public StateMachine(params State<T>[] states)
    {
        States = new Dictionary<T, State<T>>();
        
        foreach (var state in states)
        {
            RegisterState(state);
        }
    }

    public void Run(T initialStateType)
    {
        SwitchState(initialStateType);
    }

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

    public bool TrySwitchToPrevious()
    {
        if (PreviousState == null) return false;
        
        SwitchState(PreviousState.StateType);
        return true;
    }
    
    public void Dispose()
    {
        States.Clear();
        CurrentState?.OnExit();
        CurrentState = null;
    }

    private void RegisterState(State<T> state)
    {
        state.StateMachine = this;
        if(States.TryAdd(state.StateType, state)) return;
        throw new ArgumentException($"State {state.StateType} already exists in the state machine");
    }
}