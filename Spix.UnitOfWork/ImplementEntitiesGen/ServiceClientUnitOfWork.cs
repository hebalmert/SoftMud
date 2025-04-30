using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesGen;
using Spix.UnitOfWork.InterfacesEntitiesGen;

namespace Spix.UnitOfWork.ImplementEntitiesGen;

public class ServiceClientUnitOfWork : IServiceClientUnitOfWork
{
    private readonly IServiceClientService _serviceClientService;

    public ServiceClientUnitOfWork(IServiceClientService serviceClientService)
    {
        _serviceClientService = serviceClientService;
    }

    public async Task<ActionResponse<IEnumerable<ServiceClient>>> GetAsync(PaginationDTO pagination, string email) => await _serviceClientService.GetAsync(pagination, email);

    public async Task<ActionResponse<ServiceClient>> GetAsync(Guid id) => await _serviceClientService.GetAsync(id);

    public async Task<ActionResponse<ServiceClient>> UpdateAsync(ServiceClient modelo) => await _serviceClientService.UpdateAsync(modelo);

    public async Task<ActionResponse<ServiceClient>> AddAsync(ServiceClient modelo, string email) => await _serviceClientService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _serviceClientService.DeleteAsync(id);
}