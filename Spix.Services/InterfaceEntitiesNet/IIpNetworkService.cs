using Spix.Core.EntitiesNet;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfaceEntitiesNet;

public interface IIpNetworkService
{
    Task<ActionResponse<IEnumerable<IpNetwork>>> ComboAsync(string email, Guid? id = null);

    Task<ActionResponse<IEnumerable<IpNetwork>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<IpNetwork>> GetAsync(Guid id);

    Task<ActionResponse<IpNetwork>> UpdateAsync(IpNetwork modelo);

    Task<ActionResponse<IpNetwork>> AddAsync(IpNetwork modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}