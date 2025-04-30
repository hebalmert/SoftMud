using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesEntitiesGen;

public interface IServiceCategoryUnitOfWork
{
    Task<ActionResponse<IEnumerable<ServiceCategory>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<ServiceCategory>> GetAsync(Guid id);

    Task<ActionResponse<ServiceCategory>> UpdateAsync(ServiceCategory modelo);

    Task<ActionResponse<ServiceCategory>> AddAsync(ServiceCategory modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}