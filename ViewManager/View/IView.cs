namespace ViewManager.View;

public interface IView
{
    public bool IsDisplayed { get; }
    public bool IsHidden { get; }
    public bool IsBeingDisplayed { get; }
    public bool IsBeingHidden { get; }

    public event Action<IView> OnDisplay;
    public event Action<IView> OnHide;
    
    public Task Display(bool instant = false);
    public Task Hide(bool instant = false);
}