namespace Interface.Sender;

using Modell.Order;
public interface INotificationSender
{
    void Send(Order order, string message);
}
