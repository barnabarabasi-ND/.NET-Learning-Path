using System.Net.Mail;

namespace OrderProcessingDiDemo.Application.Orders.Validation;

public class OrderValidator : IOrderValidator
{
    private readonly List<string> _errors = [];

    public OrderValidationResult Validate(CreateOrderCommand command)
    {
        ValidateCustomerEmail(command.CustomerEmail);
        ValidateLines(command.Lines);

        return new(_errors);
    }

    private void ValidateCustomerEmail(string customerEmail)
    {
        if (string.IsNullOrWhiteSpace(customerEmail))
        {
            _errors.Add("CustomerEmail is required.");
            return;
        }

        if (!MailAddress.TryCreate(customerEmail, out _))
        {
            _errors.Add("CustomerEmail must be a valid email address.");
        }
    }

    private void ValidateLines(IReadOnlyCollection<CreateOrderLineCommand> lines)
    {
        if (lines.Count == 0)
        {
            _errors.Add("At least one order line is required.");
            return;
        }

        foreach (var line in lines)
        {
            if (line.ProductId <= 0)
            {
                _errors.Add("ProductId must be greater than zero.");
            }

            if (line.Quantity <= 0)
            {
                _errors.Add($"Quantity must be greater than zero for product '{line.ProductId}'.");
            }
        }
    }
}
