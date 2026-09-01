using Dependency_Injection.Notification;
using Dependency_Injection.Repositories;
using Dependency_Injection.Service;
using Interface.Repository;
using Interface.Sender;
using Modell.Order;

IOrderRepository repository = //new SqlOrderRepository();
                              new FakeOrderRepository();
INotificationSender notification =
                                    //new EmailSender();
                                    // new SmsSender();
                                    new FakeSender();

var orderService = new OrderService(
                                    repository,
                                    notification);
var order = new Order
{
    Id = 123,
    CustomerEmail = "john@example.com",
    CustomerPhone = "+40740123123"
};

orderService.PlaceOrder(order);
