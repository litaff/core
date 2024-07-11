namespace ViewManager;

using View;

/// <summary>
/// Manages views, which can contain multiple unique panels.
/// </summary>
[Serializable]
public class ViewManager
{
    protected readonly List<IView> Views;
    protected IView? CurrentView;

    public ViewManager(List<IView> views)
    {
        if (views == null || views.Count == 0)
        {
            throw new ArgumentException("[ViewManager] Views list is null or empty.");
        }
        
        Views = views;
        CurrentView = null;
    }
    
    public async Task<T> GetView<T>(bool instant = false) where T : IView
    {
        var typeViews = Views.OfType<T>().ToList();

        switch (typeViews.Count)
        {
            case 0:
                throw new ArgumentException($"[ViewManager] View of type {typeof(T).Name} not found.");
            case > 1:
                throw new ArgumentException($"[ViewManager] Multiple views of type {typeof(T).Name} found.");
        }

        var view = typeViews.First();

        if (CurrentView != null)
        {
            await CurrentView.Hide(instant);
        }
        CurrentView = view;
        await CurrentView.Display(instant);
        
        return view;
    }
}