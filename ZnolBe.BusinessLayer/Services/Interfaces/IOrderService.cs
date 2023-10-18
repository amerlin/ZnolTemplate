using ZnolBe.Shared.Models;
using ZnolBe.Shared.Models.Requests;

namespace ZnolBe.BusinessLayer.Services.Interfaces;
public interface IOrderService
{
    Task<IEnumerable<Order>> GetListAsync();
    Task<Order> SaveAsync(SaveOrderRequest order);
}