using OrderProcessingDiDemo.Domain.Orders;

namespace OrderProcessingDiDemo.Application.Orders;

public record OrderResult(
    int Id,
    string CustomerEmail,
    OrderStatus Status,
    DateTimeOffset CreatedAt,
    IReadOnlyCollection<OrderLineResult> Lines
)
{
    public static OrderResult From(Order order)
    {
        return new(
            order.Id,
            order.CustomerEmail,
            order.Status,
            order.CreatedAt,
            [.. order.Lines.Select(OrderLineResult.From)]
        );
    }
}

public record OrderLineResult(
    int ProductId,
    int Quantity
)
{
    public static OrderLineResult From(OrderLine line)
    {
        return new(line.ProductId, line.Quantity);
    }
}
