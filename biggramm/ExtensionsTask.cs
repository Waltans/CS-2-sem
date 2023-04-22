using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
    public static double Median(this IEnumerable<double> items)
    {
        var itemList = items.ToList();
        itemList.Sort();
        return DoMedian(itemList);
    }

    private static double DoMedian(List<double> itemList)
    {
        if (!itemList.Any())
            throw new InvalidOperationException();
        if (itemList.Count() % 2 != 0)
            return itemList[(itemList.Count() - 1) / 2];

        return (itemList[itemList.Count() / 2] + itemList[itemList.Count() / 2 - 1]) / 2;
    }

    public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
    {
        if (items is null)
            throw new InvalidOperationException();
        T previous = default;
        var first = true;
        foreach (var i in items)
        {
            if (first)
            {
                previous = i;
                first = false;
            }
            else
            {
                yield return new ValueTuple<T, T>(previous, i);
                previous = i;
            }
        }
    }
}