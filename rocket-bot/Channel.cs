using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace rocket_bot;

public class Channel<T> where T : class
{
    private readonly List<T?> list = new();
    private readonly ReaderWriterLockSlim rwLock = new();

    public T? this[int index]
    {
        get
        {
            rwLock.EnterReadLock();
            try
            {
                return index >= 0 && index < list.Count ? list[index] : null;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }
        set
        {
            rwLock.EnterWriteLock();
            try
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
            finally
            {
                rwLock.ExitWriteLock();
            }
        }
    }

    public T? LastItem()
    {
        rwLock.EnterReadLock();
        try
        {
            return list.Count > 0 ? list[^1] : null;
        }
        finally
        {
            rwLock.ExitReadLock();
        }
    }

    public void AppendIfLastItemIsUnchanged(T? item, T knownLastItem)
    {
        rwLock.EnterWriteLock();
        try
        {
            if (knownLastItem == list.LastOrDefault())
                list.Add(item);
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
    }

    public int Count
    {
        get
        {
            rwLock.EnterReadLock();
            try
            {
                return list.Count;
            }
            finally
            {
                rwLock.ExitReadLock();
            }
        }
    }
}