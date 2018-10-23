using System;
using System.Threading;

namespace Generator.Core
{
    public static class RandomFactory
    {
        private static int tickCountOnStart = Environment.TickCount;

        public static Random Create()
        {
            var random = new Random(Interlocked.Increment(ref tickCountOnStart));
            return random;
        }
    }
}
