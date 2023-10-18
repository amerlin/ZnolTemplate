using ZnolBe.Shared.Enums;

namespace ZnolBe.Shared.Models;
public class Order
{
    public DateTime Date { get; set; }
    public OrderStatus Status { get; set; }
    public double TotalPrice { get; set; }
}
