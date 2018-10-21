using System;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 1)
            {
                var count = uint.Parse(args[0]);
                var gen = new Generator();
                var seq = gen.CreateSequence(count);

                foreach (var str in seq)
                {
                    Console.WriteLine(str);
                }
            }
        }
    }
}
