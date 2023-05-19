using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
    public List<Point> FindPathToCompleteGoal(State state)
    {
        var remainingChests = new HashSet<Point>(state.Chests);
        var pathFinder = new DijkstraPathFinder();
        var pathToGoal = new List<Point>();

        double totalCost = 0;
        var currentPosition = state.Position;

        for (var i = 0; i < state.Goal; i++)
        {
            var shortestPath = pathFinder.GetPathsByDijkstra(state, currentPosition, remainingChests).FirstOrDefault();
            if (shortestPath == null)
                return new List<Point>();

            totalCost += shortestPath.Cost;
            currentPosition = shortestPath.End;

            if (state.Energy < totalCost)
                return new List<Point>();

            AddPathToGoal(state, remainingChests, shortestPath, pathToGoal);
        }

        return pathToGoal;
    }

    private static void AddPathToGoal(State state, HashSet<Point> remainingChests, PathWithCost shortestPath,
        List<Point> pathToGoal)
    {
        remainingChests.Remove(shortestPath.End);
        pathToGoal.AddRange(shortestPath.Path.Skip(1));
        state.Scores++;
    }
}