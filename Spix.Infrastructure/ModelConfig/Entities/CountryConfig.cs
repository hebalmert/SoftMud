using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spix.Core.Entities;

namespace Spix.Infrastructure.ModelConfig.Entities;

public class CountryConfig : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(e => e.CountryId);
        builder.HasIndex(e => e.Name).IsUnique();
    }
}