using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spix.Core.EntitiesData;

namespace Spix.Infrastructure.ModelConfig.EntitiesData;

public class OperationConfig : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.HasKey(e => e.OperationId);
        builder.HasIndex(e => e.OperationName).IsUnique();
    }
}