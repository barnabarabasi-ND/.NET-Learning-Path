namespace OrderProcessingDiDemo.Application.Orders.Validation;

public class OrderValidationException(
    IReadOnlyCollection<string> errors
) : Exception("The order command is invalid.")
{
    public IReadOnlyCollection<string> Errors { get; } = errors;
}
