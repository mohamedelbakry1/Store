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
    internal class ProductTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.Property(p => p.Name).HasColumnType("nvarchar").HasMaxLength(100);

            builder.HasMany(t => t.Products)
                   .WithOne(p => p.Type)
                   .HasForeignKey(p => p.TypeId);
        }
    }
}
