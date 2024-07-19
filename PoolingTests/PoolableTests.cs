namespace PoolingTests;

using NUnit.Framework;
using Pooling;

[TestFixture]
public class PoolableTests
{
    private Pooling<Poolable> pooling;
    private Poolable standalonePoolable;
    
    [SetUp]
    public void SetUp()
    {
        pooling = new Pooling<Poolable>();
        standalonePoolable = new Poolable();
    }

    [Test]
    public void Free_InvokesOnFreed()
    {
        var wasInvoked = false;
        standalonePoolable.OnFreed += _ => wasInvoked = true;
        standalonePoolable.Free();
        Assert.That(wasInvoked, Is.True);
    }
}