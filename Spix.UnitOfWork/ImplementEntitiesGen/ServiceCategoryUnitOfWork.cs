using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesGen;
using Spix.UnitOfWork.InterfacesEntitiesGen;

namespace Spix.UnitOfWork.ImplementEntitiesGen;

public class ServiceCategoryUnitOfWork : IServiceCategoryUnitOfWork
{
    private readonly IServiceCategoryService _serviceCategoryService;

    public ServiceCategoryUnitOfWork(IServiceCategoryService serviceCategoryService)
    {
        _serviceCategoryService = serviceCategoryService;
    }

    public async Task<ActionResponse<IEnumerable<ServiceCategory>>> ComboAsync(string email) => await _serviceCategoryService.ComboAsync(email);

    public async Task<ActionResponse<IEnumerable<ServiceCategory>>> GetAsync(PaginationDTO pagination, string email) => await _serviceCategoryService.GetAsync(pagination, email);

    public async Task<ActionResponse<ServiceCategory>> GetAsync(Guid id) => await _serviceCategoryService.GetAsync(id);

    public async Task<ActionResponse<ServiceCategory>> UpdateAsync(ServiceCategory modelo) => await _serviceCategoryService.UpdateAsync(modelo);

    public async Task<ActionResponse<ServiceCategory>> AddAsync(ServiceCategory modelo, string email) => await _serviceCategoryService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _serviceCategoryService.DeleteAsync(id);
}