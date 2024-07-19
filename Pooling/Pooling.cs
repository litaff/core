namespace Pooling;

public class Pooling<T> where T : IPoolable, new()
{
    protected readonly List<T> AvailablePoolables = new();
    protected readonly List<T> ActivePoolables = new();
    
    /// <summary>
    /// Returns cached poolable or create a new one, if no are available.
    /// </summary>
    public T GetPoolable()
    {
        var item = GetAvailableItem();
        ActivePoolables.Add(item);
        
        item.OnFreed += FreePoolable;
        return item;
    }

    /// <summary>
    /// Calls <see cref="IPoolable.Free"/> on all <see cref="ActivePoolables"/>.
    /// </summary>
    public void FreeAll()
    {
        foreach (var activePoolable in ActivePoolables.ToList())
        {
            activePoolable.Free();
        }
    }

    /// <summary>
    /// Frees a passed poolable.
    /// </summary>
    /// <exception cref="ArgumentException">Throws when passed poolable can't be cast to T.</exception>
    private void FreePoolable(IPoolable poolable)
    {
        if (poolable is not T castPoolable)
        {
            throw new ArgumentException("[Pooling] Tired to free poolable, but could not cast to T!");
        }

        poolable.OnFreed -= FreePoolable;
        ActivePoolables.Remove(castPoolable);
        AvailablePoolables.Add(castPoolable);
    }

    /// <summary>
    /// Removes first available poolable from <see cref="AvailablePoolables"/> and returns it.
    /// If there is no available poolable, a new object is passed. 
    /// </summary>
    private T GetAvailableItem()
    {
        if (!AvailablePoolables.Any()) return new T();
        var item = AvailablePoolables.First();
        AvailablePoolables.Remove(item);
        return item;
    }
}