using MiniDeepThought.Strategies;

using NUnit.Framework.Legacy; //for ClassicAssert

namespace MiniDeepThought.Tests
{
    public class SlowCountStrategyTests
    {
        [Test]
        public async Task GenerateAnswerAsync_Should_Return42()
        {
            // Arrange
            var strategy = new SlowCountStrategy();
            var progress = new Progress<int>();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await strategy.GenerateAnswerAsync(progress, null, cancellationToken);

            // Assert
            ClassicAssert.AreEqual("42", result);
        }

        [Test]
        public async Task GenerateAnswerAsync_Should_ReportProgress100()
        {
            // Arrange
            var strategy = new SlowCountStrategy();
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
            var strategy = new SlowCountStrategy();
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
