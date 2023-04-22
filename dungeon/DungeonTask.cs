using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class DungeonTask
{
    private static List<Point>? GetShortestPath(
        IEnumerable<Tuple<SinglyLinkedList<Point>?, SinglyLinkedList<Point>?>> paths)
    {
        var enumerable = paths as Tuple<SinglyLinkedList<Point>, SinglyLinkedList<Point>>[] ?? paths.ToArray()!;
        if (!enumerable.Any()) return null;

        var shortestPath = enumerable.First(x =>
            x.Item1.Length + x.Item2.Length == enumerable.Min(y => y.Item1.Length + y.Item2.Length));

        return shortestPath.Item1.Reverse()
            .Concat(shortestPath.Item2.Reverse())
            .ToList();
    }

    private static MoveDirection[] GetDirections(List<Point> points)
    {
        var moveDirection = new List<MoveDirection>();

        for (var i = 1; i < points.Count; i++)
        {
            if (points[i].X == 1 + points[i - 1].X)
                moveDirection.Add(MoveDirection.Right);
            if (points[i].X == -1 + points[i - 1].X)
                moveDirection.Add(MoveDirection.Left);
            if (points[i].Y == 1 + points[i - 1].Y)
                moveDirection.Add(MoveDirection.Down);
            if (points[i].Y == -1 + points[i - 1].Y)
                moveDirection.Add(MoveDirection.Up);
        }

        return moveDirection.ToArray();
    }


    public static MoveDirection[] FindShortestPath(Map map)
    {
        var pathFromStartToExit = BfsTask.FindPaths(map, map.InitialPosition, new[] { map.Exit }).FirstOrDefault();

        if (pathFromStartToExit == null) return Array.Empty<MoveDirection>();
        if (map.Chests.Any(chest => pathFromStartToExit.Contains(chest)))
            return GetDirections(pathFromStartToExit.Reverse().ToList());

        var pathsFromStartToChests = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
        var pathsFromChestsToExit = pathsFromStartToChests
            .Select(path =>
                Tuple.Create(path, BfsTask.FindPaths(map, path.Value, new[] { map.Exit }).FirstOrDefault()));
        var shortestPath = GetShortestPath(pathsFromChestsToExit);

        return GetDirections(shortestPath ?? pathFromStartToExit.Reverse().ToList());
    }
}