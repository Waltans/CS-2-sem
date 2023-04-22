using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
    public Rocket GetNextMove(Rocket rocket)
    {
        var tasks = CreateTasks(rocket);
        var results = Task.WhenAll(tasks).GetAwaiter().GetResult();
        var (turn, _) = results.MaxBy(t => t.Score);
        return rocket.Move(turn, level);
    }

    public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
    {
        return Enumerable.Range(0, threadsCount)
            .Select(_ =>
                Task.Run(() => SearchBestMove(rocket, new Random(random.Next()), iterationsCount / threadsCount)))
            .ToList();
    }
}