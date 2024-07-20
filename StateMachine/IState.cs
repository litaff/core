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
    public abstract T StateType { get; }
    
    /// <summary>
    /// Called when the state is entered.
    /// </summary>
    public virtual void OnEnter() { }
    
    /// <summary>
    /// Called when the state is exited.
    /// </summary>
    public virtual void OnExit() { }
}