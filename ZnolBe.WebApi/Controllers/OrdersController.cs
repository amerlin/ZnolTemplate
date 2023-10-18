using Microsoft.AspNetCore.Mvc;
using ZnolBe.BusinessLayer.Services.Interfaces;
using ZnolBe.Shared.Models.Requests;

namespace ZnolBe.WebApi.Controllers;

/// <summary>
/// Orders Controller
/// </summary>
public class OrdersController : ControllerBase
{
    private readonly IOrderService orderService;

    /// <summary>
    /// Costructor
    /// </summary>
    /// <param name="orderService">Order Service</param>
    public OrdersController(IOrderService orderService)
    {
        this.orderService = orderService;
    }

    /// <summary>
    /// Get all orders
    /// </summary>
    /// <returns>Return a list of orders</returns>
    [HttpGet]
    public async Task<IActionResult> GetListAsync()
    {
        var orders = await orderService.GetListAsync();
        return Ok(orders);
    }

    /// <summary>
    /// Create or update an orders
    /// </summary>
    /// <param name="order">Order model</param>
    /// <returns>Order updated</returns>
    [HttpPost]
    public async Task<IActionResult> SaveAsync(SaveOrderRequest order)
    {
        var savedOrder = await orderService.SaveAsync(order);
        return Ok(savedOrder);
    }
}