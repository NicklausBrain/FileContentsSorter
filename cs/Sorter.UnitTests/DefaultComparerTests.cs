using Sorter.Core;
using Xunit;

namespace Sorter.UnitTests
{
    public class DefaultComparerTests
    {
        [Fact]
        public void Compare_ForNullArguments_ThrowsNoExceptions()
        {
            var comparer = new DefaultComparer();

            var difference = comparer.Compare(null, null);

            Assert.Equal(0, difference);
        }

        [Fact]
        public void Compare_ForEmptyArguments_ThrowsNoExceptions()
        {
            var comparer = new DefaultComparer();

            var difference = comparer.Compare(string.Empty, string.Empty);

            Assert.Equal(0, difference);
        }

        [Fact]
        public void Compare_ForLineWithoutStringButNumber_TakesNumberForZero()
        {
            var comparer = new DefaultComparer();

            var difference = comparer.Compare("3. Hello World", " Hello World");

            Assert.Equal(1, difference);
        }

        [Fact]
        public void Compare_ForLineWithoutStringButNumber_TakesNumberForZero2()
        {
            var comparer = new DefaultComparer();

            var difference = comparer.Compare(" Hello World", "3. Hello World");

            Assert.Equal(-1, difference);
        }

        [Fact]
        public void Compare_ForLineWithoutNumber_ComparesStrings()
        {
            var comparer = new DefaultComparer();

            var difference = comparer.Compare("Beaver", "Chipmunk");

            Assert.Equal(-1, difference);
        }
    }
}
