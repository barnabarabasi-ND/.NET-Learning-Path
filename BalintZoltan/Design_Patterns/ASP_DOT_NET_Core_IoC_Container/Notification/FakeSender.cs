namespace ASP_DOT_NET_Core_IoC_Container.Notification;
using ASP_DOT_NET_Core_IoC_Container.Interfaces;
using ASP_DOT_NET_Core_IoC_Container.Models;
public class FakeSender : INotificationSender
{
    public string? LastRecipient { get; private set; }

    public string? LastMessage { get; private set; }

    public void Send(Order order, string message)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        LastRecipient = $"{order.CustomerEmail} {order.CustomerPhone}";
        Console.WriteLine("Last Recipient :" + LastRecipient);
        LastMessage = message;
        Console.WriteLine("Last Message :" + LastMessage);
    }
}

