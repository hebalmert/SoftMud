using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesEntitiesGen;

public interface IRegisterService
{
    Task<ActionResponse<IEnumerable<Register>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Register>> GetAsync(Guid id);

    Task<ActionResponse<Register>> UpdateAsync(Register modelo);

    Task<ActionResponse<Register>> AddAsync(Register modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}