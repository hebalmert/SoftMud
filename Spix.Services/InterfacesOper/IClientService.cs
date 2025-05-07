using Spix.Core.EntitiesNet;
using Spix.Core.EntitiesOper;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesOper;

public interface IClientService
{
    Task<ActionResponse<IEnumerable<Client>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Client>> GetAsync(Guid id);

    Task<ActionResponse<Client>> UpdateAsync(Client modelo, string frontUrl);

    Task<ActionResponse<Client>> AddAsync(Client modelo, string email, string frontUrl);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}