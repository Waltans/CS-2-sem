using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
    public List<Point> FindPathToCompleteGoal(State state)
    {
        var dijkstra = new DijkstraPathFinder();
        var unvisitedChests = new HashSet<Point>(state.Chests);
        var pathToGoal = new List<Point>();
        var currentCell = state.Position;
        var currentEnergy = 0;
        var visitedChestsCount = 0;

        while (visitedChestsCount < state.Goal && currentEnergy < state.Energy)
        {
            var nextChestPath = dijkstra.GetPathsByDijkstra(state, currentCell, unvisitedChests)
                .Where(p => p != null && unvisitedChests.Contains(p.End))
                .OrderBy(p => p.Cost)
                .FirstOrDefault();

            if (nextChestPath == null)
                return new List<Point>();

            unvisitedChests.Remove(nextChestPath.End);
            visitedChestsCount += 1;
            currentEnergy += nextChestPath.Cost;
            currentCell = nextChestPath.End;
            pathToGoal.AddRange(nextChestPath.Path.Skip(1));
        }

        if (visitedChestsCount == state.Goal && currentEnergy <= state.Energy)
            return pathToGoal;

        return new List<Point>();
    }
}