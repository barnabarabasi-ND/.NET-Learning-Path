namespace OrderProcessingDiDemo.Application.Orders.Validation;

public interface IOrderValidator
{
    OrderValidationResult Validate(CreateOrderCommand command);
}
