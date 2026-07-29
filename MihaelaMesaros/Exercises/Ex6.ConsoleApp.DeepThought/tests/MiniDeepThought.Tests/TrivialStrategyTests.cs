using MiniDeepThought.Strategies;

namespace MiniDeepThought.Tests
{
    public class TrivialStrategyTests
    {
        //[Fact] //no parameters

        //[Theory] //same test with different parameters
        //[InlineData(2, 3, 5)]
        //Assert //checks the result

        //[Theory]
        //[ClassData(typeof(TestData))] //complex data

        // GenerateAnswerAsync method must return "42" regardless of the input question.
        [Fact]
        public async Task GenerateAnswerAsync_Should_Return42()
        {
            // Arrange
            var strategy = new TrivialStrategy();

            var progress = new Progress<int>();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await strategy.GenerateAnswerAsync(progress, "dummy question", cancellationToken);

            // Assert
            Assert.Equal("42", result);
        }

        // GenerateAnswerAsync method must not depend on the input question.
        [Fact]
        public async Task GenerateAnswerAsync_Should_NotDependOnQuestion()
        {
            var strategy = new TrivialStrategy();

            var progress = new Progress<int>();
            var cancellationToken = CancellationToken.None;

            var result1 = await strategy.GenerateAnswerAsync(progress, "dummy question 1", cancellationToken);
            var result2 = await strategy.GenerateAnswerAsync(progress, "dummy question 2", cancellationToken);

            Assert.Equal(result1, result2);
        }


    }
}
