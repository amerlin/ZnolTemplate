using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZnolBe.DataAccessLayer.Configurations.Common;
using ZnolBe.DataAccessLayer.Entities;

namespace ZnolBe.DataAccessLayer.Configurations;
public class PersonConfiguration : BaseEntityConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);

        builder.ToTable("Person");
        builder.Property(x=>x.Name).IsRequired().HasMaxLength(50);
    }
}
