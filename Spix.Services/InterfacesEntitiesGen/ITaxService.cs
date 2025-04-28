using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesEntitiesGen;

public interface ITaxService
{
    Task<ActionResponse<IEnumerable<Tax>>> ComboAsync(string email);

    Task<ActionResponse<IEnumerable<Tax>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Tax>> GetAsync(Guid id);

    Task<ActionResponse<Tax>> UpdateAsync(Tax modelo);

    Task<ActionResponse<Tax>> AddAsync(Tax modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}