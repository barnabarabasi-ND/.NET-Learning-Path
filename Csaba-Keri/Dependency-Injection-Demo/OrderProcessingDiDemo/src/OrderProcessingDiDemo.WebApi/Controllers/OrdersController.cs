using Microsoft.AspNetCore.Mvc;
using OrderProcessingDiDemo.Application.Orders;
using OrderProcessingDiDemo.Application.Orders.Validation;
using OrderProcessingDiDemo.WebApi.Contracts;

namespace OrderProcessingDiDemo.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder(
        CreateOrderRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var command = request.ToCommand();

            var result = await _orderService.CreateAsync(command, cancellationToken);

            var response = result.ToResponse();

            return CreatedAtAction(
                nameof(GetOrderById),
                new { id = response.Id },
                response
            );
        }
        catch (OrderValidationException exception)
        {
            foreach (var error in exception.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }

            return ValidationProblem(ModelState);
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderResponse>> GetOrderById(
        int id,
        CancellationToken cancellationToken
    )
    {
        var result = await _orderService.GetByIdAsync(id, cancellationToken);

        return result is null
            ? NotFound()
            : Ok(result.ToResponse());
    }
}
