using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Rivals
{
    public class RivalsTask
    {
        private static readonly Point[] Steps = new Point[]
        {
            new Point(0, 1),
            new Point(0, -1),
            new Point(1, 0),
            new Point(-1, 0)
        };

        public static IEnumerable<OwnedLocation> AssignOwners(Map map)
        {
            var ownedLocations = map.Players
                .Select((p, i) => new OwnedLocation(i, p, 0))
                .ToList();

            var visitedLocations = new HashSet<Point>(map.Players);

            foreach (var ownedLocation in ownedLocations)
            {
                yield return ownedLocation;
            }

            while (ownedLocations.Any())
            {
                var currentLocation = ownedLocations.First();
                ownedLocations.RemoveAt(0);

                foreach (var step in Steps)
                {
                    var nextPosition = new Point(currentLocation.Location.X + step.X, currentLocation.Location.Y + step.Y);

                    if (IsInvalidMove(nextPosition, map, visitedLocations))
                    {
                        continue;
                    }

                    var nextOwnedLocation = new OwnedLocation(currentLocation.Owner, nextPosition, currentLocation.Distance + 1);
                    ownedLocations.Add(nextOwnedLocation);
                    visitedLocations.Add(nextOwnedLocation.Location);
                    yield return nextOwnedLocation;
                }
            }
        }

        private static bool IsInvalidMove(Point nextPosition, Map map, HashSet<Point> visitedLocations)
        {
            return !map.InBounds(nextPosition)
                || map.Maze[nextPosition.X, nextPosition.Y] == MapCell.Wall
                || visitedLocations.Contains(nextPosition);
        }
    }
}
