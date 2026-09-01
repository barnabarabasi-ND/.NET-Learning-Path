using Modell.NotificationFactory;
using Modell.OrderService;

var factory = new NotificationFactory();
var order = new OrderService(factory);
var notificationTypes = new List<String>(){"sms","email","test",/*"invalid"*/};

foreach (var notificationType in notificationTypes)
    order.CompleteOrder(notificationType);

