using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class NotGreedyPathFinder : IPathFinder
{
    public List<Point> FindPathToCompleteGoal(State state)
    {
        var dijkstraPathFinder = new DijkstraPathFinder();
        var result = null as List<Point>;
        var bestChestsCount = 0;
        var searchStack = new Stack<(Point position, int energy, List<Point> path, List<Point> chestsLeft)>();

        searchStack.Push((state.Position, state.Energy, new List<Point>(), state.Chests.ToList()));

        result = Result(state, searchStack, bestChestsCount, dijkstraPathFinder, result);

        return result;
    }

    private static List<Point>? Result(State state,
        Stack<(Point position, int energy, List<Point> path, List<Point> chestsLeft)> searchStack, int bestChestsCount,
        DijkstraPathFinder dijkstraPathFinder,
        List<Point>? result)
    {
        while (searchStack.Count > 0 && bestChestsCount < state.Chests.Count)
        {
            var (position, energy, path, chestsLeft) = searchStack.Pop();
            foreach (var newPath in dijkstraPathFinder.GetPathsByDijkstra(state, position, chestsLeft))
            {
                if (newPath != null && newPath.Cost > energy)
                    continue;

                if (newPath == null) continue;
                var newEnergy = energy - newPath.Cost;
                var newPathList = path.Concat(newPath.Path.Skip(1)).ToList();
                var newChestsLeft = chestsLeft.Except(new[] { newPath.End }).ToList();

                searchStack.Push((newPath.End, newEnergy, newPathList, newChestsLeft));
            }

            var chestsTakenCount = state.Chests.Count - chestsLeft.Count;
            if (chestsTakenCount <= bestChestsCount) continue;
            result = path;
            bestChestsCount = chestsTakenCount;
        }

        return result;
    }
}