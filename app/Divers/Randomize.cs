using System;
using System.Threading;
using System.Threading.Tasks;

namespace app.Divers
{
    public static class Randomize
    {
        private static int _seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> Random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));

        private static readonly object LockObj = new object();

        public static Task<int> GetRandomInt(int min, int max)
        {
            lock (LockObj)
            {
                return min <= max
                    ? Task.FromResult(Random.Value.Next(min, max))
                    : Task.FromResult(Random.Value.Next(max, min));
            }
        }

        public static Task<double> GetRandomNumber()
        {
            lock (LockObj)
            {
                return Task.FromResult(Random.Value.NextDouble());
            }
        }
    }
}