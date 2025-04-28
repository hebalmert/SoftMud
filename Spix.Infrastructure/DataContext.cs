using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spix.Core.EntitesSoftSec;
using Spix.Core.Entities;
using Spix.Core.EntitiesData;
using Spix.Core.EntitiesGen;
using System.Reflection;

namespace Spix.Infrastructure;

public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    //Manejo de UserRoles por Usuario

    public DbSet<UserRoleDetails> UserRoleDetails => Set<UserRoleDetails>();

    //Entities

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<State> States => Set<State>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<SoftPlan> SoftPlans => Set<SoftPlan>();
    public DbSet<Manager> Managers => Set<Manager>();
    public DbSet<Corporation> Corporations => Set<Corporation>();

    //EntitiesData

    public DbSet<FrecuencyType> FrecuencyTypes => Set<FrecuencyType>();
    public DbSet<Frecuency> Frecuencies => Set<Frecuency>();
    public DbSet<Operation> Operations => Set<Operation>();
    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<Security> Securities => Set<Security>();
    public DbSet<HotSpotType> HotSpotTypes => Set<HotSpotType>();
    public DbSet<ChainType> ChainTypes => Set<ChainType>();

    //EntitiesGen

    public DbSet<Zone> Zones => Set<Zone>();
    public DbSet<Mark> Marks => Set<Mark>();
    public DbSet<MarkModel> MarkModels => Set<MarkModel>();
    public DbSet<Tax> Taxes => Set<Tax>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<Product> Products => Set<Product>();

    //EntitiesSoftSec

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<UsuarioRole> UsuarioRoles => Set<UsuarioRole>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //Para tomar los calores de ConfigEntities
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}