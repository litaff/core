namespace Pooling;

public interface IPoolable
{
    /// <summary>
    /// Invoked when this poolable is freed.
    /// </summary>
    public event Action<IPoolable>? OnFreed;

    /// <summary>
    /// Frees this poolable.
    /// </summary>
    public void Free();
}