using System;
using System.Collections.Generic;
using System.Linq;

namespace yield
{
    public static class MovingMaxTask
    {
        public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var masse = new LinkedList<double>();
            var queue = new Queue<double>();
            
            foreach (var dataPoint in data)
            {
                if (queue.Count >= windowWidth)
                {
                    if (masse.Count == 0)
                    {
                        queue.Dequeue();
                    }
                    else
                    {
                        if (masse.First != null && Math.Abs(masse.First.Value - queue.Dequeue()) < 1e-12)
                            masse.RemoveFirst();
                    }
                }

                queue.Enqueue(dataPoint.OriginalY);
                
                while (masse.Last != null && masse.Count > 0 && masse.Last.Value <= dataPoint.OriginalY)
                {
                    masse.RemoveLast();
                }
                
                masse.AddLast(dataPoint.OriginalY);
                
                if (masse.First == null) continue;
                
                yield return dataPoint.WithMaxY(masse.First.Value);
            }
        }
    }
}