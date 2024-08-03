namespace ViewManagerTests.View;

using NUnit.Framework;
using ViewManager.View;

[TestFixture]
public class ViewTests
{
    private View view;
    
    [SetUp]
    public void SetUp()
    {
        view = new View();
    }
    
    [Test]
    public async Task Display_InitiallyHidden_MakesViewDisplayed()
    {
        await view.Hide();
        Assert.Multiple(() =>
        {
            Assert.That(view.IsDisplayed, Is.False);
            Assert.That(view.IsHidden, Is.True);
            Assert.That(view.IsBeingHidden, Is.False);
        });
        await view.Display();
        Assert.Multiple(() =>
        {
            Assert.That(view.IsDisplayed, Is.True);
            Assert.That(view.IsHidden, Is.False);
            Assert.That(view.IsBeingDisplayed, Is.False);
        });
    }

    [Test]
    public async Task Hide_InitiallyDisplayed_MakesViewHidden()
    {
        await view.Display();
        Assert.Multiple(() =>
        {
            Assert.That(view.IsDisplayed, Is.True);
            Assert.That(view.IsHidden, Is.False);
            Assert.That(view.IsBeingDisplayed, Is.False);
        });
        await view.Hide();
        Assert.Multiple(() =>
        {
            Assert.That(view.IsDisplayed, Is.False);
            Assert.That(view.IsHidden, Is.True);
            Assert.That(view.IsBeingHidden, Is.False);
        });
    }
    
    [Test]
    public async Task Display_WhenAlreadyDisplayed_DoesNothing()
    {
        await view.Display();
        var initiallyDisplayed = view.IsDisplayed;
        await view.Display();
        Assert.Multiple(() =>
        {
            Assert.That(view.IsDisplayed, Is.EqualTo(initiallyDisplayed));
            Assert.That(view.IsDisplayed, Is.True);
            Assert.That(view.IsHidden, Is.False);
            Assert.That(view.IsBeingDisplayed, Is.False);
        });
    }
    
    [Test]
    public async Task Hide_WhenAlreadyHidden_DoesNothing()
    {
        await view.Hide();
        var initiallyHidden = view.IsHidden;
        await view.Hide();
        Assert.Multiple(() =>
        {
            Assert.That(view.IsHidden, Is.EqualTo(initiallyHidden));
            Assert.That(view.IsDisplayed, Is.False);
            Assert.That(view.IsHidden, Is.True);
            Assert.That(view.IsBeingHidden, Is.False);
        });
    }
}