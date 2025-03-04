namespace ViewManagerTests;

using Moq;
using NUnit.Framework;
using ViewManager;
using ViewManager.View;

[TestFixture]
public class ViewManagerTests
{
    private ViewManager viewManager;
    private List<IView> views;
    
    [SetUp]
    public void SetUp()
    {
        views =
        [
            new Mock<IView>().Object,
            new Mock<IView>().Object
        ];
        viewManager = new ViewManager(views);
    }

    [Test]
    public void GetView_ThrowsArgumentException_WhenNoViewOfTypeFound()
    {
        Assert.Throws<ArgumentException>(() => viewManager.GetView<UnknownView>());
    }
    
    [Test]
    public void GetView_ThrowsArgumentException_WhenMultipleViewsOfTypeFound()
    {
        Assert.Throws<ArgumentException>(() => viewManager.GetView<IView>());
    }
    
    [Test]
    public void GetView_ReturnsUniqueView()
    {
        var mockView = new Mock<IView>();
        viewManager = new ViewManager([mockView.Object]);
        
        var view = viewManager.GetView<IView>();
        
        Assert.That(view, Is.EqualTo(mockView.Object));
    }
    
    public class UnknownView : IView
    {
        public bool IsDisplayed { get; }
        public bool IsHidden { get; }
        public bool IsBeingDisplayed { get; }
        public bool IsBeingHidden { get; }
        public event Action<IView>? OnDisplay;
        public event Action<IView>? OnHide;
        public Task Display(bool instant = false)
        {
            throw new NotImplementedException();
        }

        public Task Hide(bool instant = false)
        {
            throw new NotImplementedException();
        }
    } 
}
