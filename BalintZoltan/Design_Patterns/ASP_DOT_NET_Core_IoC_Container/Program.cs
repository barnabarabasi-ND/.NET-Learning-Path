using Microsoft.Extensions.DependencyInjection;
using Dependency_Injection.Notification;
using Dependency_Injection.Repositories;
using Dependency_Injection.Service;
using Interface.Repository;
using Interface.Sender;
using Model.Order;

var services = new ServiceCollection();

services.AddSingleton<IOrderRepository, SqlOrderRepository>();
//services.AddSingleton<IOrderRepository, FakeOrderRepository>();

services.AddSingleton<INotificationSender, EmailSender>();
//services.AddSingleton<INotificationSender, SmsSender>();
//services.AddSingleton<INotificationSender, FakeSender>();

services.AddTransient<OrderService>();

using var serviceProvider = services.BuildServiceProvider();

var orderService = serviceProvider.GetRequiredService<OrderService>();
var order = new Order
{
    Id = 123,
    CustomerEmail = "john@example.com",
    CustomerPhone = "+40740123123"
};

orderService.PlaceOrder(order);
