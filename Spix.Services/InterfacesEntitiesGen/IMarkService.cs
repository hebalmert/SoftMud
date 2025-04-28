using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesEntitiesGen;

public interface IMarkService
{
    Task<ActionResponse<IEnumerable<Mark>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Mark>> GetAsync(Guid id);

    Task<ActionResponse<Mark>> UpdateAsync(Mark modelo);

    Task<ActionResponse<Mark>> AddAsync(Mark modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}