namespace ViewManagerTests.View.Panel;

using NUnit.Framework;
using ViewManager.View.Panel;

[TestFixture]
public class PanelTests
{
    private Panel panel;
    
    [SetUp]
    public void SetUp()
    {
        panel = new Panel("testPanel");
    }
    
    [Test]
    public void Constructor_WithValidId_SetsIdProperty()
    {
        Assert.That(panel.Id, Is.EqualTo("testPanel"));
    }
}