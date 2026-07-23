using MiniDeepThought.Strategies;

namespace MiniDeepThought.Tests
{
    public class TrivialStrategyTests
    {
        // GenerateAnswerAsync method must return "42" regardless of the input question.
        [Test]
        public async Task GenerateAnswerAsync_Should_Return42()
        {
            // Arrange
            var strategy = new TrivialStrategy();

            var progress = new Progress<int>();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await strategy.GenerateAnswerAsync(progress, "dummy question", cancellationToken);

            // Assert
            Assert.That(result, Is.EqualTo("42"));
        }

        // GenerateAnswerAsync method must not depend on the input question.
        [Test]
        public async Task GenerateAnswerAsync_Should_NotDependOnQuestion()
        {
            var strategy = new TrivialStrategy();

            var progress = new Progress<int>();
            var cancellationToken = CancellationToken.None;

            var result1 = await strategy.GenerateAnswerAsync(progress, "dummy question 1", cancellationToken);
            var result2 = await strategy.GenerateAnswerAsync(progress, "dummy question 2", cancellationToken);

            Assert.That(result1, Is.EqualTo(result2));
        }


    }
}
