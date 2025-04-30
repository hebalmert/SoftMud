using Spix.Core.EntitiesGen;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesGen;
using Spix.UnitOfWork.InterfacesEntitiesGen;

namespace Spix.UnitOfWork.ImplementEntitiesGen;

public class PlanUnitOfWork : IPlanUnitOfWork
{
    private readonly IPlanService _planService;

    public PlanUnitOfWork(IPlanService planService)
    {
        _planService = planService;
    }

    public async Task<ActionResponse<IEnumerable<EnumItemModel>>> GetComboUpAsync() => await _planService.GetComboUpAsync();

    public async Task<ActionResponse<IEnumerable<EnumItemModel>>> GetComboDownAsync() => await _planService.GetComboDownAsync();

    public async Task<ActionResponse<IEnumerable<Plan>>> GetAsync(PaginationDTO pagination, string email) => await _planService.GetAsync(pagination, email);

    public async Task<ActionResponse<Plan>> GetAsync(Guid id) => await _planService.GetAsync(id);

    public async Task<ActionResponse<Plan>> UpdateAsync(Plan modelo) => await _planService.UpdateAsync(modelo);

    public async Task<ActionResponse<Plan>> AddAsync(Plan modelo, string email) => await _planService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _planService.DeleteAsync(id);
}