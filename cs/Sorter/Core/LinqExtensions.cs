using System;
using System.Collections.Generic;

namespace Sorter.Core
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Merge<T>(this IEnumerable<T> seqA, IEnumerable<T> seqB) where T : IComparable
        {
            IEnumerator<T> enumeratorA = seqA.GetEnumerator();
            IEnumerator<T> enumeratorB = seqB.GetEnumerator();

            T a, b;
            bool hasA, hasB;

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
