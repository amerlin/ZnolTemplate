namespace ZnolBe.DataAccessLayer.Entities.Common;
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreateOn { get; set; }
    public DateTime UpdateOn { get; set; }
    public DateTime? DeleteOn { get; set; }
}
