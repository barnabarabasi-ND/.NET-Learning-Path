using OrderProcessingDiDemo.Application.Orders;

namespace OrderProcessingDiDemo.WebApi.Contracts;

public static class OrderMappings
{
    public static CreateOrderCommand ToCommand(this CreateOrderRequest request)
    {
        var lines = (request.Lines ?? [])
            .Select(ToCommand)
            .ToArray();

        return new(
            request.CustomerEmail ?? string.Empty,
            lines
        );
    }

    public static OrderResponse ToResponse(this OrderResult result)
    {
        var lines = result.Lines
            .Select(ToResponse)
            .ToArray();

        return new(
            result.Id,
            result.CustomerEmail,
            result.Status,
            result.CreatedAt,
            lines
        );
    }

    private static CreateOrderLineCommand ToCommand(CreateOrderLineRequest request)
    {
        return new(request.ProductId, request.Quantity);
    }

    private static OrderLineResponse ToResponse(OrderLineResult result)
    {
        return new(result.ProductId, result.Quantity);
    }
}
