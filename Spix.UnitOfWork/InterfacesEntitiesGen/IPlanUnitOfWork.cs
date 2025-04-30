using Spix.Core.EntitiesGen;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesEntitiesGen;

public interface IPlanUnitOfWork
{
    Task<ActionResponse<IEnumerable<EnumItemModel>>> GetComboUpAsync();

    Task<ActionResponse<IEnumerable<EnumItemModel>>> GetComboDownAsync();

    Task<ActionResponse<IEnumerable<Plan>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Plan>> GetAsync(Guid id);

    Task<ActionResponse<Plan>> UpdateAsync(Plan modelo);

    Task<ActionResponse<Plan>> AddAsync(Plan modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}