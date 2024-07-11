namespace ViewManagerTests;

using NUnit.Framework;
using ViewManager;
using ViewManager.View.Panel;

[TestFixture]
public class PanelManagerTests
{
    private PanelManager panelManager;
    private List<IPanel> panels;
    
    [SetUp]
    public void SetUp()
    {
        panels = new List<IPanel>
        {
            new Panel("Panel1"),
            new Panel("Panel2"),
        };
        panelManager = new PanelManager(panels);
    }
    
    [Test]
    public void Constructor_WithValidPanels_SetsPanelsProperty()
    {
        Assert.That(panelManager.GetPanels<IPanel>(), Has.Count.EqualTo(panels.Count));
    }

    [Test]
    public void GetPanel_WithType_ReturnsFirstPanelOfType()
    {
        var panel = panelManager.GetPanel<IPanel>();
        Assert.That(panel.Id, Is.EqualTo("Panel1"));
    }
    
    [Test]
    public void GetPanel_WithTypeAndId_ReturnsCorrectPanel()
    {
        var panel = panelManager.GetPanel<IPanel>("Panel2");
        Assert.That(panel.Id, Is.EqualTo("Panel2"));
    }
    
    [Test]
    public void GetPanels_WithType_ReturnsAllPanelsOfType()
    {
        var retrievedPanels = panelManager.GetPanels<IPanel>();
        Assert.That(retrievedPanels, Has.Count.EqualTo(2));
    }
    
    [Test]
    public void Constructor_WithNullPanels_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => panelManager = new PanelManager(null!));
    }
    
    [Test]
    public void Constructor_WithEmptyPanels_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => panelManager = new PanelManager(new List<IPanel>()));
    }
    
    [Test]
    public void GetPanel_WithUnregisteredType_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => panelManager.GetPanel<UnregisteredPanel>());
    }
    
    [Test]
    public void GetPanel_WithUnregisteredId_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => panelManager.GetPanel<IPanel>("UnregisteredPanel"));
    }
}

public class UnregisteredPanel : Panel
{
    public UnregisteredPanel(string id) : base(id) { }
}