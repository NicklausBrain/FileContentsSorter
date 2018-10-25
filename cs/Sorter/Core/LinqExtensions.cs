using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sorter.Core
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Merge<T>(
            this IEnumerable<T> seqA,
            IEnumerable<T> seqB,
            IComparer<T> comparer = null) where T : IComparable
        {
            Func<T, T, int> compare = comparer != null
                ? comparer.Compare
                : (Func<T, T, int>)DefaultComparison;

            IEnumerator<T> enumeratorA = seqA.GetEnumerator();
            IEnumerator<T> enumeratorB = seqB.GetEnumerator();

            T a = default(T), b = default(T);
            bool hasA = false, hasB = false;

            Next(enumeratorA, ref hasA, ref a);
            Next(enumeratorB, ref hasB, ref b);

            do
            {
                if (hasA && hasB)
                {
                    if (compare(a, b) < 0)
                    {
                        yield return a;
                        Next(enumeratorA, ref hasA, ref a);
                    }
                    else
                    {
                        yield return b;
                        Next(enumeratorB, ref hasB, ref b);
                    }
                }
                else if (hasA)
                {
                    yield return a;
                    Next(enumeratorA, ref hasA, ref a);
                }
                else if (hasB)
                {
                    yield return b;
                    Next(enumeratorB, ref hasB, ref b);
                }
            }
            while (hasA || hasB);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Next<T>(IEnumerator<T> enumerator, ref bool hasValue, ref T value)
        {
            hasValue = enumerator.MoveNext();
            value = hasValue ? enumerator.Current : default(T);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int DefaultComparison<T>(T x, T y) where T : IComparable
        {
            return x.CompareTo(y);
        }
    }
}
