using AutoMapper;
using ZnolBe.Shared.Models;
using ZnolBe.Shared.Models.Requests;
using Entities = ZnolBe.DataAccessLayer.Entities;

namespace ZnolBe.BusinessLayer.MapperProfile;
public class OrderMapperProfile: Profile
{
    public OrderMapperProfile()
    {
        CreateMap<Entities.Order, Order>();

        CreateMap<SaveOrderRequest, Entities.Order>();
    }
}
