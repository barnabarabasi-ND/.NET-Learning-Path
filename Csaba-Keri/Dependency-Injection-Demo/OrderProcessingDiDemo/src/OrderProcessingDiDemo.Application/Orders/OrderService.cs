using OrderProcessingDiDemo.Application.Abstractions.Persistence;
using OrderProcessingDiDemo.Application.Abstractions.Time;
using OrderProcessingDiDemo.Application.Orders.Validation;
using OrderProcessingDiDemo.Domain.Orders;

namespace OrderProcessingDiDemo.Application.Orders;

public class OrderService(
    IOrderRepository orderRepository,
    IOrderValidator orderValidator,
    IClock clock
) : IOrderService
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IOrderValidator _orderValidator = orderValidator;
    private readonly IClock _clock = clock;

    public async Task<OrderResult> CreateAsync(
        CreateOrderCommand command,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(command);

        ValidateAndThrowIfFailed(command);

        var lines = command.Lines
            .Select(line =>
                new OrderLine(line.ProductId, line.Quantity)
            )
            .ToList();

        var order = Order.Create(
            command.CustomerEmail,
            lines,
            _clock.UtcNow
        );

        await _orderRepository.AddAsync(order, cancellationToken);

        return OrderResult.From(order);
    }

    public async Task<OrderResult?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);

        return order is null
            ? null
            : OrderResult.From(order);
    }

    private void ValidateAndThrowIfFailed(CreateOrderCommand command)
    {
        var validationResult = _orderValidator.Validate(command);

        if (!validationResult.IsValid)
        {
            throw new OrderValidationException(validationResult.Errors);
        }
    }
}
