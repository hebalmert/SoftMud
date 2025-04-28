using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesEntitiesGen;

public interface IMarkModelService
{
    Task<ActionResponse<IEnumerable<MarkModel>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<MarkModel>> GetAsync(Guid id);

    Task<ActionResponse<MarkModel>> UpdateAsync(MarkModel modelo);

    Task<ActionResponse<MarkModel>> AddAsync(MarkModel modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}