namespace ViewManagerTests.View;

using NUnit.Framework;
using ViewManager.View;

[TestFixture]
public class ViewTests
{
    private MockView view;
    
    [SetUp]
    public void SetUp()
    {
        view = new MockView();
    }

    [Test]
    public async Task Display_CallsBeforeDisplay()
    {
        Assert.That(view.BeforeDisplayCalled, Is.False);

        await view.Display(true);
        
        Assert.That(view.BeforeDisplayCalled, Is.True);
    }

    [Test]
    public async Task Display_SetsIsHidden_ToFalse()
    {
        Assert.That(view.IsHidden, Is.False);

        await view.Display(true);
        
        Assert.That(view.IsHidden, Is.False);
    }

    [Test]
    public void Display_SetsIsBeingDisplayed_ToTrue_BeforeDisplaySequence()
    {
        Assert.That(view.IsBeingDisplayed, Is.False);

        _ = view.Display();
        
        Assert.That(view.IsBeingDisplayed, Is.True);
    }

    [Test]
    public async Task Display_CallsDisplaySequence()
    {
        Assert.That(view.DisplaySequenceCalled, Is.False);

        await view.Display(true);
        
        Assert.That(view.DisplaySequenceCalled, Is.True);
    }

    [Test]
    public async Task Display_SetsIsBeingDisplayed_ToFalse_AfterDisplaySequence()
    {
        Assert.That(view.IsBeingDisplayed, Is.False);

        var task = view.Display();
        
        Assert.That(view.IsBeingDisplayed, Is.True);
        await task;
        Assert.That(view.IsBeingDisplayed, Is.False);
    }

    [Test]
    public async Task Display_SetsIsDisplayed_ToTrue()
    {
        Assert.That(view.IsDisplayed, Is.False);

        await view.Display(true);
        
        Assert.That(view.IsDisplayed, Is.True);
    }

    [Test]
    public async Task Display_CallsAfterDisplay()
    {
        Assert.That(view.AfterDisplayCalled, Is.False);

        await view.Display(true);
        
        Assert.That(view.AfterDisplayCalled, Is.True);
    }

    [Test]
    public async Task Display_InvokesOnDisplay()
    {
        var called = 0;
        view.OnDisplay += view1 => called++; 
        
        await view.Display(true);
        
        Assert.That(called, Is.EqualTo(1));
    }
    
    [Test]
    public async Task Hide_CallsBeforeHide()
    {
        Assert.That(view.BeforeHideCalled, Is.False);

        await view.Hide(true);
        
        Assert.That(view.BeforeHideCalled, Is.True);
    }

    [Test]
    public async Task Hide_SetsIsDisplayed_ToFalse()
    {
        Assert.That(view.IsDisplayed, Is.False);

        await view.Hide(true);
        
        Assert.That(view.IsDisplayed, Is.False);
    }

    [Test]
    public void Hide_SetsIsBeingHidden_ToTrue_BeforeHideSequence()
    {
        Assert.That(view.IsBeingHidden, Is.False);

        _ = view.Hide();
        
        Assert.That(view.IsBeingHidden, Is.True);
    }

    [Test]
    public async Task Hide_CallsHideSequence()
    {
        Assert.That(view.HideSequenceCalled, Is.False);

        await view.Hide(true);
        
        Assert.That(view.HideSequenceCalled, Is.True);
    }

    [Test]
    public async Task Hide_SetsIsBeingHidden_ToFalse_AfterHideSequence()
    {
        Assert.That(view.IsBeingHidden, Is.False);

        var task = view.Hide();
        
        Assert.That(view.IsBeingHidden, Is.True);
        await task;
        Assert.That(view.IsBeingHidden, Is.False);
    }

    [Test]
    public async Task Hide_SetsIsHidden_ToTrue()
    {
        Assert.That(view.IsHidden, Is.False);

        await view.Hide(true);
        
        Assert.That(view.IsHidden, Is.True);
    }

    [Test]
    public async Task Hide_CallsAfterHide()
    {
        Assert.That(view.AfterHideCalled, Is.False);

        await view.Hide(true);
        
        Assert.That(view.AfterHideCalled, Is.True);
    }

    [Test]
    public async Task Hide_InvokesOnHide()
    {
        var called = 0;
        view.OnHide += view1 => called++; 
        
        await view.Hide(true);
        
        Assert.That(called, Is.EqualTo(1));
    }
    
    public class MockView : View
    {
        public bool BeforeDisplayCalled { get; private set; }
        public bool AfterDisplayCalled { get; private set; }
        public bool DisplaySequenceCalled { get; private set; }

        protected override void BeforeDisplay()
        {
            base.BeforeDisplay();
            BeforeDisplayCalled = true;
        }

        protected override void AfterDisplay()
        {
            base.AfterDisplay();
            AfterDisplayCalled = true;
        }

        protected override async Task DisplaySequence(bool instant = false)
        {
            await base.DisplaySequence(instant);
            if (!instant)
            {
                await Task.Delay(TimeSpan.FromSeconds(0.1));
            }

            DisplaySequenceCalled = true;
        }
        
        public bool BeforeHideCalled { get; private set; }
        public bool AfterHideCalled { get; private set; }
        public bool HideSequenceCalled { get; private set; }

        protected override void BeforeHide()
        {
            base.BeforeHide();
            BeforeHideCalled = true;
        }

        protected override void AfterHide()
        {
            base.AfterHide();
            AfterHideCalled = true;
        }

        protected override async Task HideSequence(bool instant = false)
        {
            await base.HideSequence(instant);
            if (!instant)
            {
                await Task.Delay(TimeSpan.FromSeconds(0.1));
            }

            HideSequenceCalled = true;
        }
    }
}