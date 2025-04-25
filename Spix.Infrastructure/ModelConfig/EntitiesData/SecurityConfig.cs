using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spix.Core.EntitiesData;

namespace Spix.Infrastructure.ModelConfig.EntitiesData;

public class SecurityConfig : IEntityTypeConfiguration<Security>
{
    public void Configure(EntityTypeBuilder<Security> builder)
    {
        builder.HasKey(e => e.SecurityId);
        builder.HasIndex(e => e.SecurityName).IsUnique();
    }
}