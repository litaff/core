namespace ViewManager.View;

/// <summary>
/// If driven by a view manager, there can be only one view active at a time. Each view can contain multiple unique panels.
/// </summary>
[Serializable]
public class View : IView
{
    public bool IsDisplayed { get; private set; }
    public bool IsHidden { get; private set; }
    public bool IsBeingDisplayed { get; private set; }
    public bool IsBeingHidden { get; private set; }

    #region Display
    
    /// <summary>
    /// Displays the view, calling <see cref="BeforeDisplay"/>, <see cref="DisplaySequence"/> and <see cref="AfterDisplay"/>.
    /// </summary>
    /// <param name="instant">Should the sequence be instant.</param>
    public async Task Display(bool instant = false)
    {
        if(IsBeingHidden || IsBeingDisplayed || IsDisplayed)
            return;
        
        await BeforeDisplay();
        await DisplaySequence(instant);
        await AfterDisplay();
    }

    /// <summary>
    /// Invokes the display logic for this view.
    /// </summary>
    /// <param name="instant">Should the sequence be instant.</param>
    protected virtual Task DisplaySequence(bool instant = false)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Is called before the display sequence starts.
    /// </summary>
    protected virtual Task BeforeDisplay()
    {
        IsHidden = false;
        IsBeingDisplayed = true;
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Is called after the display sequence ends.
    /// </summary>
    protected virtual Task AfterDisplay()
    {
        IsDisplayed = true;
        IsBeingDisplayed = false;
        
        return Task.CompletedTask;
    }

    #endregion

    #region Hide

    /// <summary>
    /// Hide the view, calling <see cref="BeforeHide"/>, <see cref="HideSequence"/> and <see cref="AfterHide"/>.
    /// </summary>
    /// <param name="instant">Should the sequence be instant.</param>
    public async Task Hide(bool instant = false)
    {
        if(IsBeingDisplayed || IsBeingHidden || IsHidden)
            return;
        
        await BeforeHide();
        await HideSequence(instant);
        await AfterHide();
    }

    /// <summary>
    /// Invokes the hide logic for this view.
    /// </summary>
    /// <param name="instant">Should the sequence be instant.</param>
    protected virtual Task HideSequence(bool instant = false)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// Is called before the hide sequence starts.
    /// </summary>
    protected virtual Task BeforeHide()
    {
        IsDisplayed = false;
        IsBeingHidden = true;
        
        return Task.CompletedTask;
    }
    
    /// <summary>
    /// Is called after the hide sequence ends.
    /// </summary>
    protected virtual Task AfterHide()
    {
        IsHidden = true;
        IsBeingHidden = false;
        
        return Task.CompletedTask;
    }

    #endregion
}