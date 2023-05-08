using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
    public List<Point> FindPathToCompleteGoal(State state)
    {
        var pathToGoal = new List<Point>();
        var chestsToCollect = state.Chests.ToHashSet();
        var currentPosition = state.Position;

        if (GetPoints(state, currentPosition, chestsToCollect, ref pathToGoal, out var points)) return points;

        return pathToGoal;
    }

    private static bool GetPoints(State initialState, Point currentPosition, HashSet<Point> chestsToCollect,
        ref List<Point> pathToGoal,
        out List<Point> points)
    {
        for (var i = 0; i < initialState.Goal; i++)
        {
            var pathToChest = new DijkstraPathFinder()
                .GetPathsByDijkstra(initialState, currentPosition, chestsToCollect).FirstOrDefault();

            if (pathToChest == null)
            {
                points = Enumerable.Empty<Point>().ToList();
                return true;
            }

            currentPosition = pathToChest.End;

            if (initialState.Energy < pathToChest.Cost)
            {
                points = Enumerable.Empty<Point>()
                    .ToList();
                return true;
            }

            chestsToCollect.Remove(pathToChest.End);
            pathToGoal =
                pathToGoal.Concat(pathToChest.Path.Skip(1))
                    .ToList();
            initialState.Scores++;
        }

        points = null;
        return false;
    }
}