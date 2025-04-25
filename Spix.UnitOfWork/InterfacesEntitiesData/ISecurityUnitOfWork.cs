using Spix.Core.EntitiesData;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesEntitiesData;

public interface ISecurityUnitOfWork
{
    Task<ActionResponse<IEnumerable<Security>>> ComboAsync();

    Task<ActionResponse<IEnumerable<Security>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<Security>> GetAsync(int id);

    Task<ActionResponse<Security>> UpdateAsync(Security modelo);

    Task<ActionResponse<Security>> AddAsync(Security modelo);

    Task<ActionResponse<bool>> DeleteAsync(int id);
}