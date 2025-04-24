using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Spix.Core.Entities;

namespace Spix.Infrastructure.ModelConfig.Entities;

public class UserRoleConfig : IEntityTypeConfiguration<UserRoleDetails>
{
    public void Configure(EntityTypeBuilder<UserRoleDetails> builder)
    {
        builder.HasIndex(e => e.UserRoleDetailsId);
        builder.HasIndex(e => new { e.UserType, e.UserId }).IsUnique();
        //Proteccion de Borrado en Cascada
        builder.HasOne(e => e.User).WithMany(e => e.UserRoleDetails).OnDelete(DeleteBehavior.Restrict);
    }
}