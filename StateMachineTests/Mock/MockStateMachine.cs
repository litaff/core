namespace StateMachineTests.Mock;

using StateMachine;

public class MockStateMachine(params State<MockStateType>[] states) : StateMachine<MockStateType>(states)
{
    public Dictionary<MockStateType, State<MockStateType>> RegisteredStates => States;
}