using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spix.Core.EntitiesGen;

namespace Spix.Infrastructure.ModelConfig.EntitiesGen;

public class DocumentTypeConfig : IEntityTypeConfiguration<DocumentType>
{
    public void Configure(EntityTypeBuilder<DocumentType> builder)
    {
        builder.HasKey(e => e.DocumentTypeId);
        builder.HasIndex(e => new { e.DocumentName, e.CorporationId }).IsUnique();
    }
}