namespace StateMachineTests.Mock.MockStates;

using StateMachine;

public class NotRegisteredState : State<MockStateType>
{
    public override MockStateType StateType => MockStateType.NotRegistered;
}