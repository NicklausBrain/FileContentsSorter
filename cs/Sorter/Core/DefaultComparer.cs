using System;
using System.Collections.Generic;

namespace Sorter.Core
{
    public class DefaultComparer : Comparer<string>
    {
        private static readonly string[] EmptyParts = { string.Empty };

        public override int Compare(string a, string b)
        {
            var partsA = a?.Split('.', 2) ?? EmptyParts;
            var partsB = b?.Split('.', 2) ?? EmptyParts;

            var strA = partsA[partsA.Length - 1];
            var strB = partsB[partsB.Length - 1];

            int strComparison = string.Compare(strA, strB, StringComparison.Ordinal);

            if (strComparison != 0)
            {
                return strComparison;
            }

            int.TryParse(partsA[0], out var aNum);
            int.TryParse(partsB[0], out var bNum);

            return aNum.CompareTo(bNum);
        }
    }
}
