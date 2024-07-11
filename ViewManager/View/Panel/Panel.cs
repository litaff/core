namespace ViewManager.View.Panel;

[Serializable]
public class Panel : View, IPanel
{
    public string Id { get; private set; }

    public Panel(string id)
    {
        Id = id;
    }
}