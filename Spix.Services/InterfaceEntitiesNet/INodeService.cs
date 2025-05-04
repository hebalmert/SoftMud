using Spix.Core.EntitiesNet;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfaceEntitiesNet;

public interface INodeService
{
    Task<ActionResponse<IEnumerable<Node>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Node>> GetAsync(Guid id);

    Task<ActionResponse<Node>> UpdateAsync(Node modelo);

    Task<ActionResponse<Node>> AddAsync(Node modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}