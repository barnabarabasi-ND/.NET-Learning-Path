using MiniDeepThought.Strategies;

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
            Assert.That(result, Is.EqualTo("42"));
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
            Assert.That(lastProgressPercent, Is.EqualTo(100));
        }

        [Test]
        public async Task GenerateAnswerAsync_ShouldThrowTaskCanceledException_WhenJobIsCancelledDuringExecution()
        {
            // Arrange
            var strategy = new SlowCountStrategy();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(100); //cancel operation after 100ms after it started

            var progress = new Progress<int>();

            // Act & Assert
            // ThrowsAsync - checks if method throws TaskCanceledException when cancellation is requested
            Assert.That(async() => await strategy.GenerateAnswerAsync(progress, null, cancellationTokenSource.Token), Throws.TypeOf<TaskCanceledException>());

        }

    }
}
