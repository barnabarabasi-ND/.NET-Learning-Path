namespace OrderProcessingDiDemo.Domain.Orders;

public class Order
{
    public int Id { get; private set; }

    public string CustomerEmail { get; private set; }

    public OrderStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    private List<OrderLine> _lines = [];

    public IReadOnlyCollection<OrderLine> Lines
    {
        get => _lines.AsReadOnly();
        private set => _lines = [.. value];
    }

    private Order()
    {
        CustomerEmail = string.Empty;
    }

    private Order(
        string customerEmail,
        IReadOnlyCollection<OrderLine> lines,
        DateTimeOffset createdAt
    )
    {
        CustomerEmail = customerEmail;
        Lines = lines;
        CreatedAt = createdAt;
        Status = OrderStatus.Created;
    }

    public static Order Create(
        string customerEmail,
        IReadOnlyCollection<OrderLine> lines,
        DateTimeOffset createdAt
    )
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(customerEmail);
        ArgumentNullException.ThrowIfNull(lines);

        if (lines.Count == 0)
        {
            throw new ArgumentException("An order must contain at least one line.");
        }

        return new(customerEmail, lines, createdAt);
    }
}
