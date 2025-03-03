namespace StateMachine;

public interface IState<T> where T : Enum
{
    /// <summary>
    /// Contains the state machine that this state belongs to.
    /// </summary>
    public IStateMachine<T>? StateMachine { get; set; } 
    /// <summary>
    /// Returns the type of this state.
    /// </summary>
    public T StateType { get; }

    /// <summary>
    /// Called when the state is entered.
    /// </summary>
    public Task OnEnter();

    /// <summary>
    /// Called when the state is exited.
    /// </summary>
    public Task OnExit();
}