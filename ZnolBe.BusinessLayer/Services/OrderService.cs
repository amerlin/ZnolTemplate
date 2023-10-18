using System.Data;
using ZnolBe.DataAccessLayer;
using Entities = ZnolBe.DataAccessLayer.Entities;
using Order = ZnolBe.Shared.Models.Order;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ZnolBe.BusinessLayer.Services.Interfaces;
using ZnolBe.Shared.Models.Requests;

namespace ZnolBe.BusinessLayer.Services;
public class OrderService : IOrderService
{
    private readonly IDataContext dataContext;
    private readonly IMapper mapper;

    public OrderService(IDataContext datacontext, IMapper mapper)
    {
        this.dataContext = datacontext;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<Order>> GetListAsync()
    {
        var query = dataContext.GetData<Entities.Order>();

        var orders = await query.OrderBy(o => o.Date)
            .ProjectTo<Order>(mapper.ConfigurationProvider)
            .ToListAsync();

        return orders;
    }

    public async Task<Order> SaveAsync(SaveOrderRequest order)
    {
        var dbOrder = order.Id != null ? await dataContext.GetData<Entities.Order>(trackingChanges: true)
            .FirstOrDefaultAsync(o => o.Id == order.Id) : null;

        if (dbOrder == null)
        {
            dbOrder = mapper.Map<Entities.Order>(order);
            dbOrder.CreateOn = DateTime.UtcNow;
            dbOrder.Status = Shared.Enums.OrderStatus.Open;
            dataContext.Insert(dbOrder);
        }
        else
        {
            mapper.Map(order, dbOrder);
            dbOrder.UpdateOn = DateTime.UtcNow;
        }

        await dataContext.SaveAsync();

        var savedOrder = mapper.Map<Order>(dbOrder);
        return savedOrder;
    }
}
