namespace StateMachineTests.Mock;

using Logger;
using StateMachine;

public class MockStateMachine(params State<MockStateType>[] states) : StateMachine<MockStateType>(new NativeLogger(), states)
{
    public Dictionary<MockStateType, State<MockStateType>> RegisteredStates => States;
}