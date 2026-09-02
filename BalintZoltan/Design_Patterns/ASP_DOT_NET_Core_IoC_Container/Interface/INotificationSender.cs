namespace Interface.Sender;

using Model.Order;
public interface INotificationSender
{
    void Send(Order order, string message);
}

