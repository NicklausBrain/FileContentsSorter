using System;
using System.Collections.Generic;

namespace Sorter.Core
{
    public class DefaultComparer : Comparer<string>
    {
        public override int Compare(string a, string b)
        {
            var aSpan = a ?? string.Empty.AsSpan();
            var bSpan = b ?? string.Empty.AsSpan();

            var aIndexOfDot = aSpan.IndexOf('.');
            var bIndexOfDot = bSpan.IndexOf('.');
            var strA = aSpan.Slice(aIndexOfDot != -1 ? aIndexOfDot + 1 : 0);
            var strB = bSpan.Slice(bIndexOfDot != -1 ? bIndexOfDot + 1 : 0);

            int strComparison = strA.CompareTo(strB, StringComparison.OrdinalIgnoreCase);

            if (strComparison != 0)
            {
                return strComparison;
            }

            int.TryParse(aSpan.Slice(0, aIndexOfDot == -1 ? 0 : aIndexOfDot), out var aNum);
            int.TryParse(bSpan.Slice(0, bIndexOfDot == -1 ? 0 : bIndexOfDot), out var bNum);

            return aNum.CompareTo(bNum);
        }
    }
}
