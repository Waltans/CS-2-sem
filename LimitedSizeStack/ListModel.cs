using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
    public List<TItem> Items { get; }
    private int UndoLimit { get; }
    private readonly LimitedSizeStack<Tuple<TItem, int, bool>> stack;
    
    public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
    {
        UndoLimit = undoLimit;
        stack = new LimitedSizeStack<Tuple<TItem, int, bool>>(undoLimit);
    }

    public ListModel(List<TItem> items, int undoLimit)
    {
        Items = items;
        UndoLimit = undoLimit;
    }

    public void AddItem(TItem item)
    {
        Items.Add(item);
        stack.Push(new Tuple<TItem, int, bool>(item, Items.Count-1, true));
    }

    public void RemoveItem(int index)
    {
        stack.Push(new Tuple<TItem, int, bool>(Items[index], index, false));
        Items.RemoveAt(index);
    }

    public bool CanUndo() => stack.Count != 0;
    
    public void Undo()
    {
        var memory = stack.Pop();
        if (memory.Item3)
            Items.Remove(memory.Item1);
        else
            Items.Insert(memory.Item2, memory.Item1);
    }
}