using FluentAssertions;
using Xunit;

namespace AsyncAwaitDecompiled
{
    public sealed class StringExtensionTests
    {
        [Theory]
        [InlineData("Hello my friend", 3)]
        [InlineData("", 0)]
        [InlineData("a b c d efg", 5)]
        public void NumberOfWords(string @string, int expectedNumberOfWords)
        {
            var actualNumberOfWords = @string.CalculateNumberOfWords();

            actualNumberOfWords.Should().Be(expectedNumberOfWords);
        }
    }
}