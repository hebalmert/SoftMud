using Spix.Services.ImplementEntities;
using Spix.Services.InterfacesEntities;
using Spix.UnitOfWork.ImplementEntities;
using Spix.UnitOfWork.InterfacesEntities;
using Spix.UnitOfWork.InterfacesSecure;
using Spix.UnitOfWork.ImplementSecure;
using Spix.Services.InterfacesSecure;
using Spix.Services.ImplementSecure;
using Spix.UnitOfWork.InterfacesEntitiesData;
using Spix.UnitOfWork.ImplementEntitiesData;
using Spix.Services.InterfacesEntitiesData;
using Spix.Services.ImplementEntitiesData;
using Spix.UnitOfWork.InterfacesEntitiesGen;
using Spix.UnitOfWork.ImplementEntitiesGen;
using Spix.Services.InterfacesEntitiesGen;
using Spix.Services.ImplementEntitiesGen;
using Spix.UnitOfWork.InterfaceEntitiesNet;
using Spix.UnitOfWork.ImplementEntitiesNet;
using Spix.Services.InterfaceEntitiesNet;
using Spix.Services.ImplementEntitiesNet;

namespace Spix.AppBack.ProgramConfig;

public static class ServiceRegistration
{
    public static void AddUnitOfWorkAndServices(IServiceCollection services)
    {
        //Entities
        services.AddScoped<ICountriesUnitOfWork, CountriesUnitOfWork>();
        services.AddScoped<ICountriesService, CountriesService>();
        services.AddScoped<IStatesUnitOfWork, StatesUnitOfWork>();
        services.AddScoped<IStatesService, StatesService>();
        services.AddScoped<ICityUnitOfWork, CityUnitOfWork>();
        services.AddScoped<ICityService, CityService>();
        services.AddScoped<ISoftPlanUnitOfWork, SoftPlanUnitOfWork>();
        services.AddScoped<ISoftPlanService, SoftPlanService>();
        services.AddScoped<ICorporationUnitOfWork, CorporationUnitOfWork>();
        services.AddScoped<ICorporationService, CorporationService>();
        services.AddScoped<IManagerUnitOfWork, ManagerUnitOfWork>();
        services.AddScoped<IManagerService, ManagerService>();
        //EntitiesData
        services.AddScoped<IChainTypesUnitOfWork, ChainTypesUnitOfWork>();
        services.AddScoped<IChainTypesService, ChainTypesService>();
        services.AddScoped<IChannelUnitOfWork, ChannelUnitOfWork>();
        services.AddScoped<IChannelService, ChannelService>();
        services.AddScoped<IFrecuencyUnitOfWork, FrecuencyUnitOfWork>();
        services.AddScoped<IFrecuencyService, FrecuencyService>();
        services.AddScoped<IFrecuencyTypeUnitOfWork, FrecuencyTypeUnitOfWork>();
        services.AddScoped<IFrecuencyTypeService, FrecuencyTypeService>();
        services.AddScoped<IHotSpotTypeUnitOfWork, HotSpotTypeUnitOfWork>();
        services.AddScoped<IHotSpotTypeService, HotSpotTypeService>();
        services.AddScoped<IOperationUnitOfWork, OperationUnitOfWork>();
        services.AddScoped<IOperationService, OperationService>();
        services.AddScoped<ISecurityUnitOfWork, SecurityUnitOfWork>();
        services.AddScoped<ISecurityService, SecurityService>();
        //EntitiesSecurities Software
        services.AddScoped<IAccountUnitOfWork, AccountUnitOfWork>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUsuarioUnitOfWork, UsuarioUnitOfWork>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IUsuarioRoleUnitOfWork, UsuarioRoleUnitOfWork>();
        services.AddScoped<IUsuarioRoleService, UsuarioRoleService>();
        //EntitiesGen
        services.AddScoped<IZoneUnitOfWork, ZoneUnitOfWork>();
        services.AddScoped<IZoneService, ZoneService>();
        services.AddScoped<IMarkUnitOfWork, MarkUnitOfWork>();
        services.AddScoped<IMarkService, MarkService>();
        services.AddScoped<IMarkModelUnitOfWork, MarkModelUnitOfWork>();
        services.AddScoped<IMarkModelService, MarkModelService>();
        services.AddScoped<ITaxUnitOfWork, TaxUnitOfWork>();
        services.AddScoped<ITaxService, TaxService>();
        services.AddScoped<IProductCategoryUnitOfWork, ProductCategoryUnitOfWork>();
        services.AddScoped<IProductCategoryService, ProductCategoryService>();
        services.AddScoped<IProductUnitOfWork, ProductUnitOfWork>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IServiceCategoryUnitOfWork, ServiceCategoryUnitOfWork>();
        services.AddScoped<IServiceCategoryService, ServiceCategoryService>();
        services.AddScoped<IServiceClientUnitOfWork, ServiceClientUnitOfWork>();
        services.AddScoped<IServiceClientService, ServiceClientService>();
        services.AddScoped<IPlanCategoryUnitOfWork, PlanCategoryUnitOfWork>();
        services.AddScoped<IPlanCategoryService, PlanCategoryService>();
        services.AddScoped<IPlanUnitOfWork, PlanUnitOfWork>();
        services.AddScoped<IPlanService, PlanService>();
        //EntitiesNet
        services.AddScoped<IIpNetworkUnitOfWork, IpNetworkUnitOfWork>();
        services.AddScoped<IIpNetworkService, IpNetworkService>();
        services.AddScoped<IIpNetUnitOfWork, IpNetUnitOfWork>();
        services.AddScoped<IIpNetService, IpNetService>();
        services.AddScoped<INodeUnitOfWork, NodeUnitOfWork>();
        services.AddScoped<INodeService, NodeService>();
    }
}