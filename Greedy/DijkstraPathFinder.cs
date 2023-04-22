using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class DijkstraData
{
    public Point Previous;
    public readonly int Difficulty;

    public DijkstraData(Point previous, int difficulty)
    {
        Previous = previous;
        Difficulty = difficulty;
    }
}

public class DijkstraPathFinder
{
    private Dictionary<Point, DijkstraData> _track;

    private readonly Point[] _arrayOfIndex =
    {
        new(1, 0),
        new(0, -1),
        new(0, 1),
        new(-1, 0)
    };

    public IEnumerable<PathWithCost?> GetPathsByDijkstra(State state, Point start, IEnumerable<Point> targets)
    {
        var chests = new HashSet<Point>(targets);
        var notVisited = new HashSet<Point> { start };
        _track = new Dictionary<Point, DijkstraData> { [start] = new(new Point(-1, -1), 0) };

        while (true)
        {
            var toOpen = GetPointForOpening(notVisited);

            if (chests.Contains(toOpen))
            {
                chests.Remove(toOpen);
                yield return new PathWithCost(_track[toOpen].Difficulty, GetPaths(toOpen).Reverse().ToArray());
            }

            if (chests.Count == 0 || notVisited.Count == 0)
                yield break;

            UpdateTrack(toOpen, state, notVisited);
        }
    }

    private void UpdateTrack(Point toOpen, State state, HashSet<Point> notVisited)
    {
        foreach (var e in GetValidPointsToCheck(toOpen, state))
        {
            var currentPrice = _track[toOpen].Difficulty + state.CellCost[e.X, e.Y];

            if (!_track.ContainsKey(e))
                notVisited.Add(e);

            if (!_track.ContainsKey(e) || _track[e].Difficulty > currentPrice)
                _track[e] = new DijkstraData(toOpen, currentPrice);
        }

        notVisited.Remove(toOpen);
    }

    private IEnumerable<Point> GetValidPointsToCheck(Point toOpen, State state)
    {
        return _arrayOfIndex.Select(index => new Point(toOpen.X + index.X, toOpen.Y + index.Y))
            .Where(point => state.InsideMap(point) && !state.IsWallAt(point));
    }

    private Point GetPointForOpening(HashSet<Point> notVisited)
    {
        var toOpen = new Point(-1, -1);
        var minDifficulty = double.PositiveInfinity;

        foreach (var point in notVisited.Where(_track.ContainsKey))
        {
            if (_track[point].Difficulty > minDifficulty)
                continue;

            minDifficulty = _track[point].Difficulty;
            toOpen = point;
        }

        return toOpen;
    }

    private IEnumerable<Point> GetPaths(Point end)
    {
        while (end != new Point(-1, -1))
        {
            yield return end;
            end = _track[end].Previous;
        }
    }
}