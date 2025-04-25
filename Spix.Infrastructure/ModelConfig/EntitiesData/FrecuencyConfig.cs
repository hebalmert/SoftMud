using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spix.Core.EntitiesData;

namespace Spix.Infrastructure.ModelConfig.EntitiesData;

public class FrecuencyConfig : IEntityTypeConfiguration<Frecuency>
{
    public void Configure(EntityTypeBuilder<Frecuency> builder)
    {
        builder.HasKey(e => e.FrecuencyId);
        builder.HasIndex(e => new { e.FrecuencyTypeId, e.FrecuencyName }).IsUnique();
        //Evitar el borrado en cascada
        builder.HasOne(e => e.FrecuencyType).WithMany(c => c.Frecuencies).OnDelete(DeleteBehavior.Restrict);
    }
}