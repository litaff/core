namespace StateMachine;

[Serializable]
public abstract class State<T> where T : Enum
{
    public IStateMachine<T>? StateMachine { get; set; } 
    public abstract T StateType { get; }
    
    public virtual void OnEnter() { }
    public virtual void OnExit() { }
}