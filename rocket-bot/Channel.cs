using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace rocket_bot;

public class Channel<T> where T : class
{
    private readonly List<T?> _list = new();
    private readonly ReaderWriterLockSlim _rwLock = new();

    public T? this[int index]
    {
        get
        {
            _rwLock.EnterReadLock();
            try
            {
                return index >= 0 && index < _list.Count ? _list[index] : null;
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }
        set
        {
            _rwLock.EnterWriteLock();
            try
            {
                if (index == _list.Count)
                {
                    _list.Add(value);
                }
                else
                {
                    _list[index] = value;
                    _list.RemoveRange(index + 1, _list.Count - index - 1);
                }
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }
    }

    public T? LastItem()
    {
        _rwLock.EnterReadLock();
        try
        {
            return _list.Count > 0 ? _list[^1] : null;
        }
        finally
        {
            _rwLock.ExitReadLock();
        }
    }

    public void AppendIfLastItemIsUnchanged(T? item, T knownLastItem)
    {
        _rwLock.EnterWriteLock();
        try
        {
            if (knownLastItem == _list.LastOrDefault())
                _list.Add(item);
        }
        finally
        {
            _rwLock.ExitWriteLock();
        }
    }

    public int Count
    {
        get
        {
            _rwLock.EnterReadLock();
            try
            {
                return _list.Count;
            }
            finally
            {
                _rwLock.ExitReadLock();
            }
        }
    }
}