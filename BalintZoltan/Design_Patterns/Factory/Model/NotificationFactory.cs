namespace Model.NotificationFactory;

using Interface.INotification;
using Model.Notifications;
public class NotificationFactory
{
    public INotification Create(string notificationType)
    {
        return notificationType switch
        {
            "email" => new EmailNotification(),
            "sms" => new SmsNotification(),
            "test" => new FakeNotification(),
            _ => throw new ArgumentException(
                $"Unknown notification type '{notificationType}'. Valid types are: email, sms, test.",
                nameof(notificationType))
        };
    }
}

