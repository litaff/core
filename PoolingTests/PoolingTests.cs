namespace PoolingTests;

using NUnit.Framework;
using Pooling;

[TestFixture]
public class PoolingTests
{
    private MockPooling pooling;
    
    [SetUp]
    public void SetUp()
    {
        pooling = new MockPooling();
    }

    [Test]
    public void GetPoolable_ReturnsNotNull()
    {
        var poolable = pooling.GetPoolable();
        Assert.That(poolable, Is.Not.Null);
    }

    [Test]
    public void GetPoolable_ReturnsNewPoolableWhenNonAreAvailable()
    {
        Assert.Multiple(() =>
        {
            Assert.That(pooling.Available, Is.Empty);
            Assert.That(pooling.Active, Is.Empty);
        });
        var poolable = pooling.GetPoolable();
        Assert.Multiple(() =>
        {
            Assert.That(poolable, Is.Not.Null);
            Assert.That(pooling.Available, Is.Empty);
            Assert.That(pooling.Active, Has.Count.EqualTo(1));
        });
    }
    
    [Test]
    public void GetPoolable_ReturnsNewPoolableWhenNonAreAvailableButActiveExist()
    {
        Assert.Multiple(() =>
        {
            Assert.That(pooling.Available, Is.Empty);
            Assert.That(pooling.Active, Is.Empty);
        });
        var poolable = pooling.GetPoolable();
        Assert.Multiple(() =>
        {
            Assert.That(poolable, Is.Not.Null);
            Assert.That(pooling.Available, Is.Empty);
            Assert.That(pooling.Active, Has.Count.EqualTo(1));
        });
        
        poolable = pooling.GetPoolable();
        Assert.Multiple(() =>
        {
            Assert.That(poolable, Is.Not.Null);
            Assert.That(pooling.Available, Is.Empty);
            Assert.That(pooling.Active, Has.Count.EqualTo(2));
        });
    }

    [Test]
    public void GetPoolable_ReturnsAvailablePoolableWhenThereAreAvailable()
    {
        Assert.Multiple(() =>
        {
            Assert.That(pooling.Available, Is.Empty);
            Assert.That(pooling.Active, Is.Empty);
        });
        var poolable = pooling.GetPoolable();
        Assert.Multiple(() =>
        {
            Assert.That(poolable, Is.Not.Null);
            Assert.That(pooling.Available, Is.Empty);
            Assert.That(pooling.Active, Has.Count.EqualTo(1));
        });
        pooling.FreeAll();
        Assert.Multiple(() =>
        {
            Assert.That(pooling.Available, Has.Count.EqualTo(1));
            Assert.That(pooling.Active, Is.Empty);
        });
        poolable = pooling.GetPoolable();
        Assert.Multiple(() =>
        {
            Assert.That(poolable, Is.Not.Null);
            Assert.That(pooling.Available, Is.Empty);
            Assert.That(pooling.Active, Has.Count.EqualTo(1));
        });
    }

    [Test]
    public void FreeAll_DoesNothingWhenNoActiveOrAvailablePoolables()
    {
        Assert.Multiple(() =>
        {
            Assert.That(pooling.Available, Is.Empty);
            Assert.That(pooling.Active, Is.Empty);
        });
        pooling.FreeAll();
        Assert.Multiple(() =>
        {
            Assert.That(pooling.Available, Is.Empty);
            Assert.That(pooling.Active, Is.Empty);
        });
    }

    [Test]
    public void FreeAll_FreesAllActivePoolables()
    {
        pooling.GetPoolable();
        pooling.GetPoolable();
        Assert.Multiple(() =>
        {
            Assert.That(pooling.Active, Is.Not.Empty);
            Assert.That(pooling.Available, Is.Empty);
        });
        pooling.FreeAll();
        Assert.Multiple(() =>
        {
            Assert.That(pooling.Active, Is.Empty);
            Assert.That(pooling.Available, Is.Not.Empty);
        });
    }

    [Test]
    public void FreeAll_DoesNothingWhenNoActivePoolablesButAvailablePoolablesExist()
    {
        pooling.GetPoolable();
        pooling.GetPoolable();
        Assert.Multiple(() =>
        {
            Assert.That(pooling.Active, Is.Not.Empty);
            Assert.That(pooling.Available, Is.Empty);
        });
        pooling.FreeAll();
        Assert.Multiple(() =>
        {
            Assert.That(pooling.Active, Is.Empty);
            Assert.That(pooling.Available, Is.Not.Empty);
        });
        pooling.FreeAll();
        Assert.Multiple(() =>
        {
            Assert.That(pooling.Active, Is.Empty);
            Assert.That(pooling.Available, Is.Not.Empty);
        });
    }

    private class MockPooling : Pooling<Poolable>
    {
        public List<Poolable> Available => AvailablePoolables;
        public List<Poolable> Active => ActivePoolables;
    }
}