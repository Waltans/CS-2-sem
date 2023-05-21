using System;
using System.Collections.Generic;
using System.Linq;

namespace DiskTree;

public abstract class DiskTreeTask
{
    public class Root
    {
        public string Name { get; }
        private readonly Dictionary<string, Root> _nodes = new();

        public Root(string name)
        {
            Name = name;
        }

        public Root GetDirection(string subRoot)
        {
            return _nodes.TryGetValue(subRoot, out var node) ? node : _nodes[subRoot] = new Root(subRoot);
        }

        public List<string> MakeConclusion(int i, List<string> list)
        {
            if (i >= 0)
                list.Add(new string(' ', i) + Name);

            i++;
            return _nodes.Values
                .OrderBy(root => root.Name, StringComparer.Ordinal)
                .Aggregate(list, (total, root) => root.MakeConclusion(i, total));
        }
    }

    public static IEnumerable<string> Solve(List<string> input)
    {
        var root = new Root("");

        foreach (var name in input)
            name.Split('\\')
                .Aggregate(root, (current, subRoot) => current.GetDirection(subRoot));

        return root.MakeConclusion(-1, new List<string>());
    }
}