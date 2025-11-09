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
            builder.Property(O => O.ShortName).HasColumnType("varchar").HasMaxLength(128);
            builder.Property(O => O.Description).HasColumnType("varchar").HasMaxLength(256);
            builder.Property(O => O.DeliveryTime).HasColumnType("varchar").HasMaxLength(128);
            builder.Property(O => O.Price).HasColumnType("decimal(18,2)");
            
        }
    }
}
