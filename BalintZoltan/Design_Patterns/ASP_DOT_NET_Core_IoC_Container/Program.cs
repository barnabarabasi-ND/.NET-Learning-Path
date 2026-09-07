using Microsoft.Extensions.DependencyInjection;
using ASP_DOT_NET_Core_IoC_Container.Notification;
using ASP_DOT_NET_Core_IoC_Container.Repositories;
using ASP_DOT_NET_Core_IoC_Container.Services;
using ASP_DOT_NET_Core_IoC_Container.Interfaces;
using ASP_DOT_NET_Core_IoC_Container.Models;

var services = new ServiceCollection();

services.AddTransient<IOrderRepository, SqlOrderRepository>();
//services.AddTransient<IOrderRepository, FakeOrderRepository>();

services.AddTransient<INotificationSender, EmailSender>();
//services.AddTransient<INotificationSender, SmsSender>();
//services.AddTransient<INotificationSender, FakeSender>();

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
