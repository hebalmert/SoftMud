using Spix.Services.ImplementContratos;
using Spix.Services.ImplementEntities;
using Spix.Services.ImplementEntitiesData;
using Spix.Services.ImplementEntitiesGen;
using Spix.Services.ImplementEntitiesNet;
using Spix.Services.ImplementOper;
using Spix.Services.ImplementSecure;
using Spix.Services.InterfaceEntitiesNet;
using Spix.Services.InterfacesContratos;
using Spix.Services.InterfacesEntities;
using Spix.Services.InterfacesEntitiesData;
using Spix.Services.InterfacesEntitiesGen;
using Spix.Services.InterfacesOper;
using Spix.Services.InterfacesSecure;
using Spix.UnitOfWork.ImplementContratos;
using Spix.UnitOfWork.ImplementEntities;
using Spix.UnitOfWork.ImplementEntitiesData;
using Spix.UnitOfWork.ImplementEntitiesGen;
using Spix.UnitOfWork.ImplementEntitiesNet;
using Spix.UnitOfWork.ImplementOper;
using Spix.UnitOfWork.ImplementSecure;
using Spix.UnitOfWork.InterfaceContratos;
using Spix.UnitOfWork.InterfaceEntitiesNet;
using Spix.UnitOfWork.InterfacesEntities;
using Spix.UnitOfWork.InterfacesEntitiesData;
using Spix.UnitOfWork.InterfacesEntitiesGen;
using Spix.UnitOfWork.InterfacesOper;
using Spix.UnitOfWork.InterfacesSecure;

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
        services.AddScoped<IDocumentTypeUnitOfWork, DocumentTypeUnitOfWork>();
        services.AddScoped<IDocumentTypeService, DocumentTypeService>();
        services.AddScoped<IRegisterUnitOfWork, RegisterUnitOfWork>();
        services.AddScoped<IRegisterService, RegisterService>();
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
        services.AddScoped<IServerUnitOfWork, ServerUnitOfWork>();
        services.AddScoped<IServerService, ServerService>();
        //EntitiesOper
        services.AddScoped<IClientUnitOfWork, ClientUnitOfWork>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IContractorUnitOfWork, ContractorUnitOfWork>();
        services.AddScoped<IContractorService, ContractorService>();
        services.AddScoped<IContractClientUnitOfWork, ContractClientUnitOfWork>();
        services.AddScoped<IContractClientService, ContractClientService>();
    }
}