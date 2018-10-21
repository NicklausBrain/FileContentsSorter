using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator
{
    public class Generator
    {
        const string RandomPoems = @"
Whose seal is that? I think I know.
Its owner is quite happy though.
Full of joy like a vivid rainbow,
I watch him laugh. I cry hello.

He gives his seal a shake,
And laughs until her belly aches.
The only other sound's the break,
Of distant waves and birds awake.

The seal is acid, empty and deep,
But he has promises to keep,
After cake and lots of sleep.
Sweet dreams come to him cheap.

He rises from his gentle bed,
With thoughts of kittens in his head,
He eats his jam with lots of bread.
Ready for the day ahead.";

        private Random random = new Random(Environment.TickCount);

        private Lazy<string[]> lazyStrings = new Lazy<string[]>(() => RandomPoems.Split('\n', '\r').ToArray());

        private string[] strings => lazyStrings.Value;

        public IEnumerable<string> CreateSequence(ulong count)
        {
            for (ulong i = 0; i < count; i++)
            {
                var rsi = random.Next(0, strings.Length);
                var rs = strings[rsi];
                var ri = random.Next(short.MinValue, short.MaxValue);
                var res = ri == rsi ? $"{""}. {rs}" : $"{ri}. {rs}";
                yield return res;
            }
        }
    }
}
