using Spix.Core.Entities;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesEntities;

public interface IManagerUnitOfWork
{
    Task<ActionResponse<IEnumerable<Manager>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<Manager>> GetAsync(int id);

    Task<ActionResponse<Manager>> UpdateAsync(Manager modelo, string frontUrl);

    Task<ActionResponse<Manager>> AddAsync(Manager modelo, string frontUrl);

    Task<ActionResponse<bool>> DeleteAsync(int id);
}