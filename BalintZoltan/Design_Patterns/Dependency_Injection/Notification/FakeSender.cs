namespace Dependency_Injection.Notification;

using Interface.Sender;
using Model.Order;

public class FakeSender : INotificationSender
{
    public string? LastRecipient { get; private set; }
    public string? LastMessage { get; private set; }

    public void Send(Order order, string message)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        LastRecipient = order.CustomerEmail;
        LastRecipient = LastRecipient + " " + order.CustomerPhone;
        Console.WriteLine("Last Recipient :" + LastRecipient);
        LastMessage = message;
        Console.WriteLine("Last Message :" + LastMessage);
    }
}