using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesGen;
using Spix.UnitOfWork.InterfacesEntitiesGen;

namespace Spix.UnitOfWork.ImplementEntitiesGen;

public class PlanCategoryUnitOfWork : IPlanCategoryUnitOfWork
{
    private readonly IPlanCategoryService _planCategoryService;

    public PlanCategoryUnitOfWork(IPlanCategoryService planCategoryService)
    {
        _planCategoryService = planCategoryService;
    }

    public async Task<ActionResponse<IEnumerable<PlanCategory>>> GetAsync(PaginationDTO pagination, string email) => await _planCategoryService.GetAsync(pagination, email);

    public async Task<ActionResponse<PlanCategory>> GetAsync(Guid id) => await _planCategoryService.GetAsync(id);

    public async Task<ActionResponse<PlanCategory>> UpdateAsync(PlanCategory modelo) => await _planCategoryService.UpdateAsync(modelo);

    public async Task<ActionResponse<PlanCategory>> AddAsync(PlanCategory modelo, string email) => await _planCategoryService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _planCategoryService.DeleteAsync(id);
}