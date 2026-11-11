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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.SubTotal).HasColumnType("decimal").HasPrecision(8, 2);
            builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(50);

            builder.OwnsOne(o => o.ShippingAddress, address =>
            {
                address.Property(s => s.Street).HasColumnType("nvarchar").HasMaxLength(50);
                address.Property(s => s.City).HasColumnType("nvarchar").HasMaxLength(50);
                address.Property(s => s.FirstName).HasColumnType("nvarchar").HasMaxLength(50);
                address.Property(s => s.LastName).HasColumnType("nvarchar").HasMaxLength(50);
                address.Property(s => s.Country).HasColumnType("nvarchar").HasMaxLength(50);
            });
        }
    }
}
