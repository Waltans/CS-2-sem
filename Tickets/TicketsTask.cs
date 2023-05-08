using System.Linq;
using System.Numerics;

namespace Tickets;

class TicketsTask
{
    public static BigInteger Solve(int ticketLength, int ticketSum)
    {
        if (ticketSum % 2 != 0) return 0;
        var halfSum = ticketSum / 2;
        var count = new BigInteger[ticketLength + 1, halfSum + 1];

        Enumerable.Range(0, ticketLength + 1)
            .ToList()
            .ForEach(i => count[i, 0] = 1);

        Enumerable.Range(1, ticketLength)
            .ToList()
            .ForEach(i => Enumerable.Range(1, halfSum)
                .ToList()
                .ForEach(j => count[i, j] = Enumerable.Range(0, 10)
                    .Where(n => j - n >= 0)
                    .Aggregate(new BigInteger(), (subtotal, n) => subtotal + count[i - 1, j - n])));

        return count[ticketLength, halfSum] * count[ticketLength, halfSum];
    }
}