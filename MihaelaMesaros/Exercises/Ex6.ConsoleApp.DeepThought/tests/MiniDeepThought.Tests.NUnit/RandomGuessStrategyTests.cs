using MiniDeepThought.Strategies;

using NUnit.Framework.Legacy; //for ClassicAssert

namespace MiniDeepThought.Tests
{
    public class RandomGuessStrategyTests
    {
        [TestCase("Question 1")]
        [TestCase("Question 1")]
        [TestCase("Question 2")]
        [TestCase(null)]
        [TestCase("")]
        public async Task GenerateAnswerAsync_Should_ReturnExpectedAnswer(string? questionText)
        {
            // Arrange
            var strategy = new RandomGuessStrategy();
            var progress = new Progress<int>();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await strategy.GenerateAnswerAsync(progress, questionText, cancellationToken);

            // Assert
            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsNotEmpty(result);
            ClassicAssert.That(result.StartsWith("Answer:"));
            ClassicAssert.That(result.Contains("Summary:"));
            if (questionText != null)
            {
                ClassicAssert.That(result.Contains(questionText));
            }
        }

        [Test]
        public async Task GenerateAnswerAsync_Should_ReportProgress100()
        {
            // Arrange
            var strategy = new RandomGuessStrategy();
            var cancellationToken = CancellationToken.None;

            int lastProgressPercent = 0;
            var progress = new Progress<int>(value => lastProgressPercent = value); // last reported progress value from progress.Report

            // Act
            await strategy.GenerateAnswerAsync(progress, null, cancellationToken);

            // Assert
            ClassicAssert.AreEqual(100, lastProgressPercent);
        }

        [Test]
        public async Task GenerateAnswerAsync_ShouldThrowTaskCanceledException_WhenJobIsCancelledDuringExecution()
        {
            // Arrange
            var strategy = new RandomGuessStrategy();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100); //cancel operation after 100ms after it started

            var progress = new Progress<int>();

            // Arrange the action to be executed by Assert.ThrowsAsync
            AsyncTestDelegate action = () => strategy.GenerateAnswerAsync(progress, null, cancellationTokenSource.Token);

            // Act & Assert
            // ThrowsAsync - checks if method throws TaskCanceledException when cancellation is requested
            Assert.ThrowsAsync<TaskCanceledException>(action);

        }

    }
}
