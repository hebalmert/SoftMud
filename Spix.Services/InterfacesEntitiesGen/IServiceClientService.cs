using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesEntitiesGen;

public interface IServiceClientService
{
    Task<ActionResponse<IEnumerable<ServiceClient>>> ComboAsync(string email, Guid id);

    Task<ActionResponse<IEnumerable<ServiceClient>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<ServiceClient>> GetAsync(Guid id);

    Task<ActionResponse<ServiceClient>> UpdateAsync(ServiceClient modelo);

    Task<ActionResponse<ServiceClient>> AddAsync(ServiceClient modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}