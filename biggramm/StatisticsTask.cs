using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public class StatisticsTask
{
    public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
    {
        var groups = visits.GroupBy(x => x.UserId);
        var times = groups.SelectMany(group => group.OrderBy(visit => visit.DateTime)
                    .Bigrams()
                    .Where(pair => pair.Item1.SlideType == slideType),
                (group, pair) =>
                    (pair.Item2.DateTime - pair.Item1.DateTime)
                    .TotalMinutes)
            .Where(time => time >= 1 && time <= 120)
            .OrderBy(time => time)
            .ToList();

        if (!times.Any()) return 0;

        var middle = times.Count / 2;
        return times.Count % 2 == 0
            ? (times[middle - 1] + times[middle]) / 2
            : times[middle];
    }
}