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
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(oi => oi.Price).HasColumnType("decimal").HasPrecision(8, 2);

            builder.OwnsOne(oi => oi.Product, p =>
            {
                p.Property(p => p.ProductName).HasColumnType("nvarchar").HasMaxLength(100);
                p.Property(p => p.PictureUrl).HasColumnType("nvarchar").HasMaxLength(200);
            });

            builder.HasOne(oi => oi.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(oi => oi.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
