using System.Net.Mail;

namespace OrderProcessingDiDemo.Application.Orders.Validation;

public class OrderValidator : IOrderValidator
{
    public OrderValidationResult Validate(CreateOrderCommand command)
    {
        var errors = new List<string>();

        ValidateCustomerEmail(command.CustomerEmail, errors);
        ValidateLines(command.Lines, errors);

        return new(errors);
    }

    private static void ValidateCustomerEmail(string customerEmail, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(customerEmail))
        {
            errors.Add("CustomerEmail is required.");
            return;
        }

        if (!MailAddress.TryCreate(customerEmail, out _))
        {
            errors.Add("CustomerEmail must be a valid email address.");
        }
    }

    private static void ValidateLines(
        IReadOnlyCollection<CreateOrderLineCommand> lines,
        List<string> errors
    )
    {
        if (lines.Count == 0)
        {
            errors.Add("At least one order line is required.");
            return;
        }

        foreach (var line in lines)
        {
            if (line.ProductId <= 0)
            {
                errors.Add("ProductId must be greater than zero.");
            }

            if (line.Quantity <= 0)
            {
                errors.Add($"Quantity must be greater than zero for product '{line.ProductId}'.");
            }
        }
    }
}
