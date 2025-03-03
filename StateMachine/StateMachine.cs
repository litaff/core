namespace StateMachine;

using Logger;

[Serializable]
public class StateMachine<T> : IStateMachine<T> where T : Enum
{
    private readonly Dictionary<T, IState<T>> states;
    
    protected ILogger Logger { get; private set; }
    public IState<T> CurrentState { get; protected set; }
    public IState<T>? PreviousState { get; protected set; }
    public IReadOnlyDictionary<T, IState<T>> States => states;
    
    public event Action<T>? OnStateChanged;
    
    public StateMachine(T initialStateType, ILogger logger, params IState<T>[] states)
    {
        Logger = logger;
        this.states = new Dictionary<T, IState<T>>();
        
        foreach (var state in states)
        {
            RegisterState(state);
        }
        
        if (!States.TryGetValue(initialStateType, out var value))
        {
            throw new ArgumentException($"State {initialStateType} does not exist in the state machine.");
        }
        
        CurrentState = value;
    }

    public async Task Run()
    {
        await EnterState(CurrentState);
    }
    
    public async Task SwitchState(T stateType)
    {
        if (CurrentState.StateType.Equals(stateType)) return;

        await ExitState(CurrentState);
        
        if (!States.TryGetValue(stateType, out var value))
        {
            throw new ArgumentException($"State {stateType} does not exist in the state machine.");
        }
        
        PreviousState = CurrentState;
        CurrentState = value;

        await EnterState(CurrentState);
    }
    
    public async void Dispose()
    {
        states.Clear();
        await ExitState(CurrentState);
        Logger.Log("Disposed.");
    }

    private void RegisterState(IState<T> state)
    {
        state.StateMachine = this;
        if (states.TryAdd(state.StateType, state))
        {
            Logger.Log($"State registered: {state.StateType}.");
            return;
        }
        throw new ArgumentException($"State {state.StateType} already exists in the state machine.");
    }

    private async Task EnterState(IState<T> state)
    {
        Logger.Log($"Entering state: {state.StateType}.");
        await state.OnEnter();
        Logger.Log($"Entered state: {state.StateType}.");
        OnStateChanged?.Invoke(state.StateType);
    }
    
    private async Task ExitState(IState<T> state)
    {
        Logger.Log($"Exiting state: {state.StateType}.");
        await state.OnExit();
        Logger.Log($"Exited state: {state.StateType}.");
    }
}