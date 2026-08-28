using OrderProcessingDiDemo.Domain.Orders;

namespace OrderProcessingDiDemo.WebApi.Contracts;

public record OrderResponse(
    int Id,
    string CustomerEmail,
    OrderStatus Status,
    DateTimeOffset CreatedAt,
    IReadOnlyCollection<OrderLineResponse> Lines
);

public record OrderLineResponse(
    int ProductId,
    int Quantity
);
