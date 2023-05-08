using System;
using System.Collections.Generic;
using System.Linq;

namespace Antiplagiarism;

public class LevenshteinCalculator
{
    private readonly Func<string, string, double> _calculate = (f, s) =>
        f == s ? 0 : TokenDistanceCalculator.GetTokenDistance(f, s);

    public List<ComparisonResult> CompareDocumentsPairwise(List<List<string>> documents)
    {
        return documents.SelectMany((doc1, index1) =>
                documents.Skip(index1 + 1).Select((doc2, index2) =>
                    new { doc1, doc2, index1, index2 }))
            .Select(pair => GetComparisonResult(pair.doc1, pair.doc2))
            .ToList();
    }

    private ComparisonResult GetComparisonResult(List<string> document1, List<string> document2)
    {
        var opt = Enumerable.Range(0, document1.Count + 1)
            .SelectMany(i => Enumerable.Range(0, document2.Count + 1)
                .Select(j => i == 0 ? j : j == 0 ? i : 0.0))
            .ToArray();

        for (var i = 1; i <= document1.Count; i++)
        for (var j = 1; j <= document2.Count; j++)
        {
            var min = new[]
            {
                opt[(i - 1) * (document2.Count + 1) + j] + 1,
                opt[i * (document2.Count + 1) + (j - 1)] + 1,
                opt[(i - 1) * (document2.Count + 1) + (j - 1)]
                + _calculate(document1[i - 1], document2[j - 1])
            }.Prepend(double.MaxValue).Min();
            opt[i * (document2.Count + 1) + j] = min;
        }

        return new ComparisonResult(
            document1, document2,
            opt[(document1.Count + 1) * (document2.Count + 1) - 1]);
    }
}