using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
    public List<Point> FindPathToCompleteGoal(State state)
    {
        var result = new List<Point>();

        if (state.Goal == 0 || state.Chests.Count == 0) return result;

        var chests = new HashSet<Point>(state.Chests);
        var finder = new DijkstraPathFinder();
        var position = state.Position;
        var energyLeft = state.Energy;

        while (chests.Any())
        {
            var pathToClosestChest = finder.GetPathsByDijkstra(state, position, chests)
                .OrderBy(path => path.Cost)
                .FirstOrDefault();

            if (pathToClosestChest == null || energyLeft < pathToClosestChest.Cost) return new List<Point>();

            result.AddRange(pathToClosestChest.Path.Skip(1));
            position = pathToClosestChest.End;
            energyLeft -= pathToClosestChest.Cost;
            chests.Remove(pathToClosestChest.End);
        }

        return result;
    }
}