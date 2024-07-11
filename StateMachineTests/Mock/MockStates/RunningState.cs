namespace StateMachineTests.Mock.MockStates;

using StateMachine;

public class RunningState : State<MockStateType>
{
    public override MockStateType StateType => MockStateType.Running;
}