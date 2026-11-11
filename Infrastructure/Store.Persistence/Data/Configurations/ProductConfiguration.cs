using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Persistence.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).HasColumnType("nvarchar").HasMaxLength(100);
            builder.Property(p => p.Description).HasColumnType("nvarchar").HasMaxLength(500);
            builder.Property(p => p.PictureUrl).HasColumnType("nvarchar").HasMaxLength(200);
            builder.Property(p => p.Price).HasColumnType("decimal").HasPrecision(18, 2);

        }
    }
}
