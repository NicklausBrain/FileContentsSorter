using System;
using System.Collections.Generic;
using System.Linq;

namespace Sorter.Core
{
    public static class LinqExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
        {
            int i = 0;
            var splits = from item in list
                         group item by i++ % parts into part
                         select part.AsEnumerable();
            return splits;
        }

        public static IEnumerable<T> Merge<T>(this IEnumerable<T> seqA, IEnumerable<T> seqB) where T : struct, IComparable
        {
            IEnumerator<T> enumeratorA = seqA.GetEnumerator();
            IEnumerator<T> enumeratorB = seqB.GetEnumerator();

            T? a = null;
            T? b = null;

            a = enumeratorA.MoveNext() ? enumeratorA.Current : (T?)null;
            b = enumeratorB.MoveNext() ? enumeratorB.Current : (T?)null;

            do
            {
                if (a.HasValue && b.HasValue)
                {
                    if (a.Value.CompareTo(b.Value) < 0)
                    {
                        yield return a.Value;
                        a = enumeratorA.MoveNext() ? enumeratorA.Current : (T?)null;
                    }
                    else
                    {
                        yield return b.Value;
                        b = enumeratorB.MoveNext() ? enumeratorB.Current : (T?)null;
                    }
                }
                else if (a.HasValue)
                {
                    yield return a.Value;
                    a = enumeratorA.MoveNext() ? enumeratorA.Current : (T?)null;
                }
                else if (b.HasValue)
                {
                    yield return b.Value;
                    b = enumeratorB.MoveNext() ? enumeratorB.Current : (T?)null;
                }
            }
            while (a.HasValue || b.HasValue);
        }
    }
}
