namespace ViewManager.View;

[Serializable]
public class View : IView
{
    public bool IsDisplayed { get; private set; }
    public bool IsHidden { get; private set; }
    public bool IsBeingDisplayed { get; private set; }
    public bool IsBeingHidden { get; private set; }
    
    public event Action<IView>? OnDisplay;
    public event Action<IView>? OnHide;

    public async Task Display(bool instant = false)
    {
        if (IsBeingHidden || IsBeingDisplayed || IsDisplayed) return;
        
        BeforeDisplay();
        IsHidden = false;
        IsBeingDisplayed = true;
        await DisplaySequence(instant);
        IsBeingDisplayed = false;
        IsDisplayed = true;
        AfterDisplay();
        OnDisplay?.Invoke(this);
    }

    protected virtual Task DisplaySequence(bool instant = false) { return Task.CompletedTask; }

    protected virtual void BeforeDisplay() { }

    protected virtual void AfterDisplay() { }

    public async Task Hide(bool instant = false)
    {
        if (IsBeingDisplayed || IsBeingHidden || IsHidden) return;
        
        BeforeHide();
        IsDisplayed = false;
        IsBeingHidden = true;
        await HideSequence(instant);
        IsBeingHidden = false;
        IsHidden = true;
        AfterHide();
        OnHide?.Invoke(this);
    }

    protected virtual Task HideSequence(bool instant = false) { return Task.CompletedTask; }

    protected virtual void BeforeHide() { }
    
    protected virtual void AfterHide() { }
}