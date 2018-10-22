using System;

namespace Sorter.Core
{
    public struct TestStruct : IComparable
    {
        public TestStruct(ulong num, string str)
        {
            this.Num = num;

            this.Str = str;
        }

        ulong Num { get; }

        string Str { get; }

        public int CompareTo(object obj)
        {
            if (obj is TestStruct)
            {
                var target = (TestStruct)obj;
                var strDiff = this.Str.CompareTo(target.Str);

                if (strDiff == 0)
                {
                    var numDiff = this.Num.CompareTo(target.Num);
                    return numDiff;
                }

                return strDiff;
            }

            throw new ArgumentException($"Object must be of type {this.GetType().Name}.");
        }

        public override string ToString()
        {
            return $"{this.Num}.{this.Str}";
        }

        public static TestStruct Parse(string str)
        {
            var parts = str.Split('.');
            ulong n;
            ulong.TryParse(parts[0], out n);

            return new TestStruct(
                n,
                parts[1]);
        }
    }
}
