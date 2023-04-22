using System.Collections.Generic;

namespace yield
{
    public static class MovingAverageTask
    {
        public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var queue = new Queue<DataPoint>();
            DataPoint points = null;
            var averageOfY = 0.0d;
            var firstElement = false;
            
            foreach (var dataPoint in addPoints(data, windowWidth, firstElement, queue, averageOfY)) 
                yield return dataPoint;
        }

        private static IEnumerable<DataPoint> addPoints(IEnumerable<DataPoint> data, int windowWidth, 
            bool firstElement, Queue<DataPoint> queue, double averageOfY)
        {
            DataPoint points;
            foreach (var point in data)
            {
                if (firstElement)
                {
                    if (queue.Count == windowWidth)
                        averageOfY -= queue.Dequeue().AvgSmoothedY;

                    queue.Enqueue(point.WithAvgSmoothedY(point.OriginalY));
                    averageOfY += point.OriginalY;
                    points = point.WithAvgSmoothedY(averageOfY / queue.Count);
                }
                else
                {
                    firstElement = true;
                    points = point.WithAvgSmoothedY(point.OriginalY);
                    averageOfY += points.AvgSmoothedY;
                    queue.Enqueue(points);
                }

                yield return points;
            }
        }
    }
}