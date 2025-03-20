namespace ExtensionsTests;

using Extensions;

[TestFixture]
public class RandomExtensionsTests
{
    private Random random;
    
    [SetUp]
    public void SetUp()
    {
        random = new Random();
    }

    [Test]
    public void NextSingleOverload_DoesNotThrow_WithValidInputs()
    {
        Assert.DoesNotThrow(() => random.NextSingle(0, 0));
    }
    
    [TestCase(1, 1)]
    [TestCase(0, 1)]
    [TestCase(1, 0)]
    [TestCase(0, 0)]
    [TestCase(-1, 0)]
    [TestCase(0, -1)]
    [TestCase(-1, -1)]
    [TestCase(-1, 1)]
    [TestCase(1, -1)]
    public void NextSingleOverload_ReturnsFromArgument_WhenLowest(float from, float to)
    {
        random = new MockRandom(0);
        
        var value = random.NextSingle(from, to);
        
        Assert.That(value, Is.EqualTo(from));
    }
    
    [TestCase(1, 1)]
    [TestCase(0, 1)]
    [TestCase(1, 0)]
    [TestCase(0, 0)]
    [TestCase(-1, 0)]
    [TestCase(0, -1)]
    [TestCase(-1, -1)]
    [TestCase(-1, 1)]
    [TestCase(1, -1)]
    public void NextSingleOverload_ReturnsAlmostToArgument_WhenHighest(float from, float to)
    {
        random = new MockRandom(BitConverter.Int32BitsToSingle(0x3F7FFFFF)); // Simulate whatever the fuck max value is...
        
        var value = random.NextSingle(from, to);
        
        Assert.That(value, Is.EqualTo(to).Within(0.00001f));
    }

    private class MockRandom(float nextSingle) : Random
    {
        public float NextSingleValue { get; } = nextSingle;

        public override float NextSingle()
        {
            return NextSingleValue;
        }
    }
}