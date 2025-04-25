using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spix.Core.EntitiesData;

namespace Spix.Infrastructure.ModelConfig.EntitiesData;

public class HotSpotTypeConfig : IEntityTypeConfiguration<HotSpotType>
{
    public void Configure(EntityTypeBuilder<HotSpotType> builder)
    {
        builder.HasKey(e => e.HotSpotTypeId);
        builder.HasIndex(e => e.TypeName).IsUnique();
    }
}