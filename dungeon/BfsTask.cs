using System;
using System.Collections.Generic;
using System.Linq;

namespace Dungeon;

public class BfsTask
{
    private static readonly Point[] Steps =
    {
        new(0, 1),
        new(0, -1),
        new(1),
        new(-1)
    };

    public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
    {
        var visited = new HashSet<Point>();
        var chestSet = new HashSet<Point>(chests);
        var startingNode = new SinglyLinkedList<Point>(start);

        var queue = new Queue<SinglyLinkedList<Point>>();
        EnqueueNode(queue, startingNode, visited);

        while (queue.Any())
        {
            var currentNode = queue.Dequeue();
            var currentPosition = currentNode.Value;

            if (IsChest(currentPosition, chestSet))
            {
                yield return currentNode;
                RemoveChest(currentPosition, chestSet);
            }

            foreach (var nextPosition in GetNeighbors(currentPosition, map, visited))
            {
                var nextNode = new SinglyLinkedList<Point>(nextPosition, currentNode);
                EnqueueNode(queue, nextNode, visited);
            }
        }
    }

    private static bool IsChest(Point position, HashSet<Point> chestSet)
    {
        if (chestSet == null) throw new ArgumentNullException(nameof(chestSet));
        return chestSet.Contains(position);
    }

    private static void RemoveChest(Point position, HashSet<Point> chestSet)
    {
        if (chestSet == null) throw new ArgumentNullException(nameof(chestSet));
        chestSet.Remove(position);
    }

    private static IEnumerable<Point> GetNeighbors(Point currentPosition, Map map, HashSet<Point> visited)
    {
        if (visited == null) throw new ArgumentNullException(nameof(visited));
        foreach (var step in Steps)
        {
            var nextPosition = new Point(currentPosition.X + step.X, currentPosition.Y + step.Y);

            if (map.InBounds(nextPosition)
                && map.Dungeon[nextPosition.X, nextPosition.Y] != MapCell.Wall
                && visited.Add(nextPosition))
                yield return nextPosition;
        }
    }

    private static void EnqueueNode(Queue<SinglyLinkedList<Point>> queue, SinglyLinkedList<Point> node,
        HashSet<Point> visited)
    {
        if (visited == null) throw new ArgumentNullException(nameof(visited));
        queue.Enqueue(node);
        visited.Add(node.Value);
    }
}