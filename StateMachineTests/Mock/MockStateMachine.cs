namespace StateMachineTests.Mock;

using Logger;
using StateMachine;

public class MockStateMachine : StateMachine<MockStateType>
{
    public Dictionary<MockStateType, IState<MockStateType>> RegisteredStates => States;
    
    public MockStateMachine(params State<MockStateType>[] states) : base(new ConsoleLogger(), states)
    {
    }
}