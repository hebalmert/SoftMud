using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spix.Core.EntitiesGen;

namespace Spix.Infrastructure.ModelConfig.EntitiesGen;

public class PlanCategoryConfig : IEntityTypeConfiguration<PlanCategory>
{
    public void Configure(EntityTypeBuilder<PlanCategory> builder)
    {
        builder.HasKey(X => X.PlanCategoryId);
        builder.HasIndex(x => new { x.PlanCategoryName, x.CorporationId }).IsUnique();
    }
}