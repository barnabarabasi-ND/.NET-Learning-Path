using MiniDeepThought.Strategies;

namespace MiniDeepThought.Tests
{
    public class SlowCountStrategyTests
    {
        [Fact]
        public async Task GenerateAnswerAsync_Should_Return42()
        {
            // Arrange
            var strategy = new SlowCountStrategy();
            var progress = new Progress<int>();
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await strategy.GenerateAnswerAsync(progress, null, cancellationToken);

            // Assert
            Assert.Equal("42", result);
        }

        [Fact]
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
            Assert.Equal(100, lastProgressPercent);
        }

        [Fact]
        public async Task GenerateAnswerAsync_ShouldThrowTaskCanceledException_WhenJobIsCancelledDuringExecution()
        {
            // Arrange
            var strategy = new SlowCountStrategy();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100); //cancel operation after 100ms after it started

            var progress = new Progress<int>();

            // Arrange the action to be executed by Assert.ThrowsAsync
            Func<Task<string>> action = () => strategy.GenerateAnswerAsync(progress, null, cancellationTokenSource.Token);

            // Act & Assert
            // ThrowsAsync - checks if method throws TaskCanceledException when cancellation is requested
            await Assert.ThrowsAsync<TaskCanceledException>(action);

        }

    }
}
