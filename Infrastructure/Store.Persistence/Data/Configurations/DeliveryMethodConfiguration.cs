using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.Data.Configurations
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(d => d.Cost).HasColumnType("decimal").HasPrecision(8, 2);
            builder.Property(d => d.ShortName).HasColumnType("nvarchar").HasMaxLength(50);
            builder.Property(d => d.Description).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(d => d.DeliveryTime).HasColumnType("nvarchar").HasMaxLength(50);

            builder.HasMany(d => d.Orders)
                   .WithOne(o => o.DeliveryMethod)
                   .HasForeignKey(o => o.DeliveryMethodId);

        }
    }
}
