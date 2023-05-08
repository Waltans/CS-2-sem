using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiplagiarism;

public static class LongestCommonSubsequenceCalculator
{
    public static List<string> Calculate(List<string> first, List<string> second)
    {
        var opt = Enumerable.Range(0, first.Count + 1)
            .Select(x => Enumerable.Repeat(0, second.Count + 1).ToArray())
            .ToArray();
        CreateOptimizationTable(first, second, opt);
        return RestoreAnswer(first, second, opt);
    }

    private static void CreateOptimizationTable(List<string> first, List<string> second, int[][] opt)
    {
        for (var i = 1; i <= first.Count; ++i)
        for (var j = 1; j <= second.Count; ++j)
            opt[i][j] = first[i - 1] == second[j - 1]
                ? opt[i - 1][j - 1] + 1
                : Math.Max(opt[i][j - 1], opt[i - 1][j]);
    }

    private static List<string> RestoreAnswer(List<string> first, List<string> second, int[][] opt)
    {
        var result = new List<string>();
        for (var (i, j) = (first.Count, second.Count); i != 0 && j != 0;)
            if (first[i - 1] == second[j - 1])
            {
                result.Add(first[i - 1]);
                i--;
                j--;
            }
            else if (opt[i][j - 1] > opt[i - 1][j])
            {
                j--;
            }
            else
            {
                i--;
            }

        result.Reverse();
        return result;
    }
}