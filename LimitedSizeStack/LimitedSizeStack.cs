using System;
using System.Collections;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    LinkedList<T> stack = new LinkedList<T>();
    int limit;
    public LimitedSizeStack(int undoLimit)
    {
        limit = undoLimit;
    }

    public void Push(T item)
    {
        if (item is null) return;
		
        if (limit == 0) return;
		
        if ((stack.Count == limit) && (stack.Count >= 1))
        {
            stack.RemoveFirst();
        }

        stack.AddLast(item);
    }

    public T Pop()
    {
        if (stack.Count == 0) throw new SystemException("Exception");
        var resultAfterPop = stack.Last.Value;
        stack.RemoveLast();
        return resultAfterPop;
    }

    public int Count => stack.Count;
}