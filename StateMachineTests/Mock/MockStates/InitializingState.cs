namespace StateMachineTests.Mock.MockStates;

using StateMachine;

public class InitializingState : State<MockStateType>
{
    public override MockStateType StateType => MockStateType.Initializing;
}