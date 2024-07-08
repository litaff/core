namespace StateMachineTests;

using Mock;
using Mock.MockStates;

[TestFixture]
public class StateMachineTests
{
    private MockStateMachine stateMachine;
    private InitializingState initializingState;
    
    [SetUp]
    public void SetUp()
    {
        initializingState = new InitializingState();
        stateMachine = new MockStateMachine(initializingState, new RunningState(), new EndingState());
    }
    
    [Test]
    public void Constructor_RegistersStates()
    {
        Assert.That(stateMachine.RegisteredStates, Has.Count.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(stateMachine.RegisteredStates.ContainsKey(MockStateType.Initializing), Is.False);
            Assert.That(stateMachine.RegisteredStates.ContainsKey(MockStateType.Running), Is.True);
            Assert.That(stateMachine.RegisteredStates.ContainsKey(MockStateType.Ending), Is.True);
        });
    }
    
    [Test]
    public void Constructor_AssignsReferenceToStateMachine()
    {
        Assert.That(initializingState.StateMachine, Is.EqualTo(stateMachine));
    }
    
    [Test]
    public void Run_SwitchesToPassedState()
    {
        stateMachine.Run(MockStateType.Initializing);
        Assert.That(stateMachine.CurrentState, Is.Not.Null);
        Assert.That(stateMachine.CurrentState.StateType, Is.EqualTo(MockStateType.Initializing));
    }
    
    [Test]
    public void SwitchState_ToDifferentState_ChangesState()
    {
        stateMachine.SwitchState(MockStateType.Running);
        Assert.That(stateMachine.CurrentState, Is.Not.Null);
        Assert.That(stateMachine.CurrentState.StateType, Is.EqualTo(MockStateType.Running));
    }
    
    [Test]
    public void SwitchState_ThrowsExceptionIfStateNotRegistered()
    {
        Assert.That(() => stateMachine.SwitchState(MockStateType.NotRegistered), Throws.Exception.TypeOf<ArgumentException>());
    }
    
    [Test]
    public void SwitchState_ToSameState_DoesNotChangeState()
    {
        stateMachine.Run(MockStateType.Initializing);
        stateMachine.SwitchState(MockStateType.Initializing);
        
        Assert.That(stateMachine.CurrentState, Is.Not.Null);
        Assert.That(stateMachine.CurrentState.StateType, Is.EqualTo(MockStateType.Initializing));
    }
    
    [Test]
    public void TrySwitchToPrevious_WhenNoPreviousState_ReturnsFalse()
    {
        Assert.That(stateMachine.TrySwitchToPrevious(), Is.False);
    }
    
    [Test]
    public void TrySwitchToPrevious_WhenPreviousState_ReturnsTrue()
    {
        stateMachine.Run(MockStateType.Initializing);
        stateMachine.SwitchState(MockStateType.Running);
        Assert.Multiple(() =>
        {
            Assert.That(stateMachine.TrySwitchToPrevious(), Is.True);
            Assert.That(stateMachine.CurrentState, Is.Not.Null);
        });
        Assert.That(stateMachine.CurrentState.StateType, Is.EqualTo(MockStateType.Initializing));
    }
    
    [Test]
    public void Dispose_ClearsStatesAndCurrentState()
    {
        stateMachine.Run(MockStateType.Initializing);
        stateMachine.Dispose();
        
        Assert.Multiple(() =>
        {
            Assert.That(stateMachine.RegisteredStates, Has.Count.EqualTo(0));
            Assert.That(stateMachine.CurrentState, Is.Null);
        });
    }
    
    [TearDown]
    public void TearDown()
    {
        stateMachine.Dispose();
    }
}