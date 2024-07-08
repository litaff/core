namespace StateMachineTests.Mock.MockStates;

using StateMachine;

public class EndingState : State<MockStateType>
{
    public override MockStateType StateType => MockStateType.Ending;
}