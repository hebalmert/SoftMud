using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spix.Core.EntitiesGen;

namespace Spix.Infrastructure.ModelConfig.EntitiesGen;

public class ProductCategoryConfig : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.HasKey(x => x.ProductCategoryId);
        builder.HasIndex(e => new { e.CorporationId, e.Name }).IsUnique();
    }
}