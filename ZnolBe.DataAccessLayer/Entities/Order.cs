using ZnolBe.DataAccessLayer.Entities.Common;
using ZnolBe.Shared.Enums;

namespace ZnolBe.DataAccessLayer.Entities;
public class Order : BaseEntity
{
    public DateTime Date { get; set; }
    public OrderStatus Status { get; set; }
    public double TotalPrice { get; set; }
}
