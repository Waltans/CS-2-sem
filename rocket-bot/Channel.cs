using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace rocket_bot;

public class Channel<T> where T : class
{
    private readonly List<T?> list = new();

    public T? this[int index]
    {
        get
        {
            lock (list)
            {
                return index >= 0 && index < list.Count ? list[index] : null;
            }
        }
        set
        {
            lock (list)
            {
                if (index == list.Count)
                {
                    list.Add(value);
                }
                else
                {
                    list[index] = value;
                    list.RemoveRange(index + 1, list.Count - index - 1);
                }
            }
        }
    }

    public T? LastItem()
    {
        lock (list)
        {
            return list.Count > 0 ? list[^1] : null;
        }
    }

    public void AppendIfLastItemIsUnchanged(T? item, T knownLastItem)
    {
        lock (list)
        {
            if (knownLastItem == list.LastOrDefault())
                list.Add(item);
        }
    }

    public int Count
    {
        get
        {
            lock (list)
            {
                return list.Count;
            }
        }
    }
}