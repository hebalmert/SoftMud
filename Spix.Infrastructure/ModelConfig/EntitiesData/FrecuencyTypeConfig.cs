using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spix.Core.EntitiesData;

namespace Spix.Infrastructure.ModelConfig.EntitiesData;

public class FrecuencyTypeConfig : IEntityTypeConfiguration<FrecuencyType>
{
    public void Configure(EntityTypeBuilder<FrecuencyType> builder)
    {
        builder.HasKey(e => e.FrecuencyTypeId);
        builder.HasIndex(e => e.TypeName).IsUnique();
    }
}