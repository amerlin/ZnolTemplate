using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZnolBe.DataAccessLayer.Configurations.Common;
using ZnolBe.DataAccessLayer.Entities;

namespace ZnolBe.DataAccessLayer.Configurations;
public class OrderConfiguration : BaseEntityConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        base.Configure(builder);

        builder.ToTable("Order");
        builder.Property(x => x.Status).HasConversion<string>();
    }
}
