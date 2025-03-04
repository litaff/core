namespace ViewManager;

using View;

[Serializable]
public class ViewManager(List<IView> views)
{
    protected readonly List<IView> Views = views;

    public T GetView<T>() where T : IView
    {
        var typeViews = Views.OfType<T>().ToList();

        switch (typeViews.Count)
        {
            case 0:
                throw new ArgumentException($"View of type {typeof(T).Name} not found.");
            case > 1:
                throw new ArgumentException($"Multiple views of type {typeof(T).Name} found.");
        }

        return typeViews.First();
    }
}