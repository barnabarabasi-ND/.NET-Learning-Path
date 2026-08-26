namespace OrderProcessingDiDemo.Application.Orders;

public record CreateOrderCommand(
    string CustomerEmail,
    IReadOnlyCollection<CreateOrderLineCommand> Lines
);

public record CreateOrderLineCommand(
    int ProductId,
    int Quantity
);
