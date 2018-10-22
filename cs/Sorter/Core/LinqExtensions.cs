using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Sorter.Core
{
    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> ParallelSplit<T>(this IEnumerable<T> seq, int parts)
        {
            int i = 0;

            var splits = seq.AsParallel()
                .GroupBy(item => Interlocked.Increment(ref i) % parts)
                .Select(part => part.AsEnumerable());

            return splits;
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> seq, int parts)
        {
            int i = 0;

            var splits = seq
                .GroupBy(item => i++ % parts)
                .Select(part => part.AsEnumerable());

            return splits;
        }

        public static IEnumerable<T> Merge<T>(this IEnumerable<T> seqA, IEnumerable<T> seqB) where T : IComparable
        {
            IEnumerator<T> enumeratorA = seqA.GetEnumerator();
            IEnumerator<T> enumeratorB = seqB.GetEnumerator();

            T a = default(T), b = default(T);
            bool hasA = false, hasB = false;

            void NextA()
            {
                hasA = enumeratorA.MoveNext();
                a = hasA ? enumeratorA.Current : default(T);
            }

            void NextB()
            {
                hasB = enumeratorB.MoveNext();
                b = hasB ? enumeratorB.Current : default(T);
            }

            NextA();
            NextB();

            do
            {
                if (hasA && hasB)
                {
                    if (a.CompareTo(b) < 0)
                    {
                        yield return a;
                        NextA();
                    }
                    else
                    {
                        yield return b;
                        NextB();
                    }
                }
                else if (hasA)
                {
                    yield return a;
                    NextA();
                }
                else if (hasB)
                {
                    yield return b;
                    NextB();
                }
            }
            while (hasA || hasB);
        }
    }
}
