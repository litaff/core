namespace ViewManagerTests;

using Logger;
using NUnit.Framework;
using ViewManager;
using ViewManager.View;

[TestFixture]
public class ViewManagerTests
{
    private MockViewManager viewManager;
    private List<IView> views;
    
    [SetUp]
    public void SetUp()
    {
        views = new List<IView>
        {
            new FirstMockView(),
            new SecondMockView(),
        };
        
        viewManager = new MockViewManager(views);
    }
    
    [Test]
    public void Constructor_WithValidViews_SetsViewsProperty()
    {
        Assert.That(viewManager.Views, Has.Count.EqualTo(2));
    }
    
    [Test]
    public async Task GetView_WithType_ReturnsCorrectView()
    {
        var view = await viewManager.GetView<SecondMockView>();
        Assert.That(view, Is.TypeOf<SecondMockView>());
    }

    [Test]
    public async Task GetView_WithType_SetsCurrentView()
    {
        await viewManager.GetView<SecondMockView>();
        Assert.That(viewManager.CurrentView, Is.Not.Null);
    }
    
    [Test]
    public void Constructor_WithNullViews_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => viewManager = new MockViewManager(null!));
    }
    
    [Test]
    public void Constructor_WithEmptyViews_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => viewManager = new MockViewManager(new List<IView>()));
    }
    
    [Test]
    public void GetView_WithUnregisteredView_ThrowsArgumentException()
    {
        Assert.ThrowsAsync<ArgumentException>(() => viewManager.GetView<UnregisteredView>());
    }
}

public class FirstMockView : global::ViewManager.View.View
{
    
}

public class SecondMockView : global::ViewManager.View.View
{
    
}

public class UnregisteredView : global::ViewManager.View.View
{
    
}

public class MockViewManager : ViewManager
{
    public new List<IView> Views => base.Views;
    public new IView? CurrentView => base.CurrentView;

    public MockViewManager(List<IView> views) : base(views, new NativeLogger())
    {
    }
}