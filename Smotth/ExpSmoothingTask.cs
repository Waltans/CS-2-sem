using System.Collections.Generic;

namespace yield
{
    public static class ExpSmoothingTask
    {
        public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
        {
            DataPoint currentPoint = null;
            var firstItem = true;
            foreach (var e in data)
            {
                if (firstItem == false)
                {
                    currentPoint = e.WithExpSmoothedY((e.OriginalY * alpha) + 
                                                      ((1 - alpha) * currentPoint.ExpSmoothedY));
                }
                else
                {
                    firstItem = false;
                    currentPoint = e.WithExpSmoothedY(e.OriginalY);
                }
                yield return currentPoint;

            }
        }
    }
}