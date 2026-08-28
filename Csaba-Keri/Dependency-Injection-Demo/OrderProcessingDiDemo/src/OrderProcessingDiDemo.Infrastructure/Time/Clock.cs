using OrderProcessingDiDemo.Application.Abstractions.Time;

namespace OrderProcessingDiDemo.Infrastructure.Time;

public class Clock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
