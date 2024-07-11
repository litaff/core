namespace ViewManager;

using View.Panel;

/// <summary>
/// Manages generic/reusable panels, like a confirmation dialog, multiple choice dialog, etc.
/// </summary>
[Serializable]
public class PanelManager
{
    private List<IPanel> panels;

    public PanelManager(List<IPanel> panels)
    {
        if (panels == null || panels.Count == 0)
        {
            throw new ArgumentException("[PanelManager] Panels list is null or empty.");
        }
        
        this.panels = panels;
    }

    public T GetPanel<T>() where T : IPanel
    {
        return GetPanels<T>().First();
    }

    public T GetPanel<T>(string id) where T : IPanel
    {
        var panel = GetPanels<T>().FirstOrDefault(x => x.Id == id);
        if(panel == null)
        {
            throw new ArgumentException($"[PanelManager] Panel of type {typeof(T).Name}, with id {id} not found.");
        }
        return panel;
    }

    public List<T> GetPanels<T>() where T : IPanel
    {
        var panel = panels.OfType<T>().ToList();

        if (!panel.Any())
        {
            throw new ArgumentException($"[PanelManager] Panel of type {typeof(T).Name} not found.");
        }
        
        return panel;
    }
}