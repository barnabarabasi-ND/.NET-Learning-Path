namespace OrderProcessingDiDemo.WebApi.Contracts;

public record CreateOrderRequest(
    string? CustomerEmail,
    IReadOnlyCollection<CreateOrderLineRequest>? Lines
);

public record CreateOrderLineRequest(
    int ProductId,
    int Quantity
);
