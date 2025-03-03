namespace StateMachineTests;

using Logger;
using Moq;
using StateMachine;

[TestFixture]
public class StateMachineTests
{
    private StateMachine<MockStateType> stateMachine;
    private MockState mockState1;
    private MockState mockState2;
    
    [SetUp]
    public void SetUp()
    {
        mockState1 = new MockState
        {
            StateType = MockStateType.Mock1
        };
        mockState2 = new MockState
        {
            StateType = MockStateType.Mock2
        };
        stateMachine = new StateMachine<MockStateType>(MockStateType.Mock1, new ConsoleLogger(), mockState1, mockState2);
    }

    [Test]
    public void Constructor_RegistersStates()
    {
        Assert.Multiple(() =>
        {
            Assert.That(stateMachine.States.ContainsKey(mockState1.StateType) &&
                            stateMachine.States.ContainsKey(mockState2.StateType));
            Assert.That(mockState1.StateMachine, Is.EqualTo(stateMachine));
            Assert.That(mockState2.StateMachine, Is.EqualTo(stateMachine));
        });
    }

    [Test]
    public void Constructor_ThrowsArgumentException_WhenDoubleRegistering()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new StateMachine<MockStateType>(MockStateType.Mock1, new ConsoleLogger(), mockState1, mockState1);
        });
    }
    
    [Test]
    public void Constructor_ThrowsArgumentException_WhenInitialStateNotRegistered()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            _ = new StateMachine<MockStateType>(MockStateType.Mock1, new ConsoleLogger(), mockState2);
        });
    }
    
    [Test]
    public void Constructor_AssignsCurrentState()
    {
        Assert.That(stateMachine.CurrentState, Is.EqualTo(mockState1));
    }
    
    [Test]
    public async Task Run_LogsStateEnter_Twice()
    {
        var logger = new Mock<ILogger>();
        stateMachine = new StateMachine<MockStateType>(MockStateType.Mock1, logger.Object, mockState1, mockState2);

        await stateMachine.Run();
        
        logger.Verify(log => log.Log($"Entering state: {MockStateType.Mock1}."), Times.Once);
        logger.Verify(log => log.Log($"Entered state: {MockStateType.Mock1}."), Times.Once);
    }
    
    [Test]
    public async Task Run_InvokedStateChangedEvent_WithInitialState()
    {
        var called = 0;
        var state = MockStateType.Mock2;
        stateMachine.OnStateChanged += type =>
        {
            state = type;
            called++;
        };

        await stateMachine.Run();
        Assert.Multiple(() =>
        {
            Assert.That(called, Is.EqualTo(1));
            Assert.That(state, Is.EqualTo(MockStateType.Mock1));
        });
    }

    [Test]
    public async Task Run_Calls_OnEnter()
    {
        var logger = new Mock<ILogger>();
        var mockState = new Mock<IState<MockStateType>>();
        mockState.Setup(mock => mock.StateType).Returns(MockStateType.Mock1);
        stateMachine = new StateMachine<MockStateType>(MockStateType.Mock1, logger.Object, mockState.Object, mockState2);

        await stateMachine.Run();
        
        mockState.Verify(mock => mock.OnEnter(), Times.Once);
    }

    [Test]
    public void Dispose_ClearsStates()
    {
        Assert.That(stateMachine.States, Is.Not.Empty);
        
        stateMachine.Dispose();
        
        Assert.That(stateMachine.States, Is.Empty);
    }
    
    [Test]
    public void Dispose_LogsDisposed()
    {
        var logger = new Mock<ILogger>();
        stateMachine = new StateMachine<MockStateType>(MockStateType.Mock1, logger.Object, mockState1, mockState2);
        
        stateMachine.Dispose();
        
        logger.Verify(log => log.Log("Disposed."), Times.Once);
    }
    
    [Test]
    public void Dispose_LogsStateExit_Twice()
    {
        var logger = new Mock<ILogger>();
        stateMachine = new StateMachine<MockStateType>(MockStateType.Mock1, logger.Object, mockState1, mockState2);

        stateMachine.Dispose();
        
        logger.Verify(log => log.Log($"Exiting state: {MockStateType.Mock1}."), Times.Once);
        logger.Verify(log => log.Log($"Exited state: {MockStateType.Mock1}."), Times.Once);
    }
    
    [Test]
    public void Dispose_Calls_OnExit()
    {
        var logger = new Mock<ILogger>();
        var mockState = new Mock<IState<MockStateType>>();
        mockState.Setup(mock => mock.StateType).Returns(MockStateType.Mock1);
        stateMachine = new StateMachine<MockStateType>(MockStateType.Mock1, logger.Object, mockState.Object, mockState2);

        stateMachine.Dispose();
        
        mockState.Verify(mock => mock.OnExit(), Times.Once);
    }
    
    [Test]
    public async Task SwitchState_DoesNotCall_OnExit_WhenSwitchingToSameState()
    {
        var logger = new Mock<ILogger>();
        var mockState = new Mock<IState<MockStateType>>();
        mockState.Setup(mock => mock.StateType).Returns(MockStateType.Mock1);
        stateMachine = new StateMachine<MockStateType>(MockStateType.Mock1, logger.Object, mockState.Object, mockState2);
        await stateMachine.Run();

        await stateMachine.SwitchState(MockStateType.Mock1);
        
        mockState.Verify(mock => mock.OnExit(), Times.Never);
    }
    
    [Test]
    public async Task SwitchState_Calls_OnExit_WhenSwitchingToDifferentState()
    {
        var logger = new Mock<ILogger>();
        var mockState = new Mock<IState<MockStateType>>();
        mockState.Setup(mock => mock.StateType).Returns(MockStateType.Mock1);
        var mockState2 = new Mock<IState<MockStateType>>();
        mockState2.Setup(mock => mock.StateType).Returns(MockStateType.Mock2);
        stateMachine = new StateMachine<MockStateType>(MockStateType.Mock1, logger.Object, mockState.Object, mockState2.Object);
        await stateMachine.Run();

        await stateMachine.SwitchState(MockStateType.Mock2);
        
        mockState.Verify(mock => mock.OnExit(), Times.Once);
    }
    
    [Test]
    public async Task SwitchState_ThrowsArgumentException_WhenSwitchingToUnregisteredState()
    {
        var logger = new Mock<ILogger>();
        var mockState = new Mock<IState<MockStateType>>();
        mockState.Setup(mock => mock.StateType).Returns(MockStateType.Mock1);
        var mockState2 = new Mock<IState<MockStateType>>();
        mockState2.Setup(mock => mock.StateType).Returns(MockStateType.Mock2);
        stateMachine = new StateMachine<MockStateType>(MockStateType.Mock1, logger.Object, mockState.Object);
        await stateMachine.Run();

        Assert.ThrowsAsync<ArgumentException>(async () => await stateMachine.SwitchState(MockStateType.Mock2));
    }
    
    [Test]
    public async Task SwitchState_SetsPreviousStateAsCurrent()
    {
        var logger = new Mock<ILogger>();
        var mockState = new Mock<IState<MockStateType>>();
        mockState.Setup(mock => mock.StateType).Returns(MockStateType.Mock1);
        var mockState2 = new Mock<IState<MockStateType>>();
        mockState2.Setup(mock => mock.StateType).Returns(MockStateType.Mock2);
        stateMachine = new StateMachine<MockStateType>(MockStateType.Mock1, logger.Object, mockState.Object, mockState2.Object);
        await stateMachine.Run();
        Assert.That(stateMachine.PreviousState, Is.Null);

        await stateMachine.SwitchState(MockStateType.Mock2);
        
        Assert.That(stateMachine.PreviousState, Is.EqualTo(mockState.Object));
    }
    
    [Test]
    public async Task SwitchState_SetsCurrentStateAsSelected()
    {
        var logger = new Mock<ILogger>();
        var mockState = new Mock<IState<MockStateType>>();
        mockState.Setup(mock => mock.StateType).Returns(MockStateType.Mock1);
        var mockState2 = new Mock<IState<MockStateType>>();
        mockState2.Setup(mock => mock.StateType).Returns(MockStateType.Mock2);
        stateMachine = new StateMachine<MockStateType>(MockStateType.Mock1, logger.Object, mockState.Object, mockState2.Object);
        await stateMachine.Run();
        Assert.That(stateMachine.CurrentState, Is.EqualTo(mockState.Object));

        await stateMachine.SwitchState(MockStateType.Mock2);
        
        Assert.That(stateMachine.CurrentState, Is.EqualTo(mockState2.Object));
    }
    
    [Test]
    public async Task SwitchState_Calls_OnEnter_WhenSwitchingToDifferentState()
    {
        var logger = new Mock<ILogger>();
        var mockState = new Mock<IState<MockStateType>>();
        mockState.Setup(mock => mock.StateType).Returns(MockStateType.Mock1);
        var mockState2 = new Mock<IState<MockStateType>>();
        mockState2.Setup(mock => mock.StateType).Returns(MockStateType.Mock2);
        stateMachine = new StateMachine<MockStateType>(MockStateType.Mock1, logger.Object, mockState.Object, mockState2.Object);
        await stateMachine.Run();

        await stateMachine.SwitchState(MockStateType.Mock2);
        
        mockState2.Verify(mock => mock.OnEnter(), Times.Once);
    }

    [TearDown]
    public void TearDown()
    {
        stateMachine.Dispose();
    }
    
    public enum MockStateType
    {
        Mock1,
        Mock2
    }

    public class MockState : IState<MockStateType>
    {
        public IStateMachine<MockStateType>? StateMachine { get; set; }
        public MockStateType StateType { get; set; }
        public Task OnEnter() { return Task.CompletedTask; }
        public Task OnExit() { return Task.CompletedTask; }
    }
}