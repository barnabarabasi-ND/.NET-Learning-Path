namespace OrderProcessingDiDemo.Application.Orders.Validation;

public class OrderValidationResult(IReadOnlyCollection<string> errors)
{
    public IReadOnlyCollection<string> Errors { get; } = errors;

    public bool IsValid => Errors.Count == 0;
}
