namespace Pooling;

/// <summary>
/// Default implementation of the <see cref="IPoolable"/> interface.
/// </summary>
public class Poolable : IPoolable
{
    public event Action<IPoolable>? OnFreed;
    
    public virtual void Free()
    {
        OnFreed?.Invoke(this);
    }
}