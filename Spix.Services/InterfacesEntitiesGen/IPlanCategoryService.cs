using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesEntitiesGen;

public interface IPlanCategoryService
{
    Task<ActionResponse<IEnumerable<PlanCategory>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<PlanCategory>> GetAsync(Guid id);

    Task<ActionResponse<PlanCategory>> UpdateAsync(PlanCategory modelo);

    Task<ActionResponse<PlanCategory>> AddAsync(PlanCategory modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}