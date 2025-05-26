using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spix.Core.EntitiesInven;

namespace Spix.Infrastructure.ModelConfig.EntitiesInven;

public class TransferConfig : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.HasKey(e => e.TransferId);
        builder.HasIndex(e => new { e.CorporationId, e.NroTransfer }).IsUnique();
        builder.Property(e => e.DateTransfer).HasColumnType("date");
        //Evitar el borrado en cascada
        builder.HasOne(e => e.Usuario).WithMany(c => c.Transfers).OnDelete(DeleteBehavior.Restrict);
    }
}