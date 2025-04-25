using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spix.Core.EntitiesData;

namespace Spix.Infrastructure.ModelConfig.EntitiesData;

public class ChainTypeConfig : IEntityTypeConfiguration<ChainType>
{
    public void Configure(EntityTypeBuilder<ChainType> builder)
    {
        builder.HasKey(e => e.ChainTypeId);
        builder.HasIndex(e => e.ChainName).IsUnique();
    }
}