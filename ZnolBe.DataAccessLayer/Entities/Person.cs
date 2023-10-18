using ZnolBe.DataAccessLayer.Entities.Common;

namespace ZnolBe.DataAccessLayer.Entities;

public class Person : BaseEntity
{
    public string Name { get; set; } = null!;
    public string City { get; set; } = null!;
}
