using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesEntitiesGen;

public interface IMarkModelUnitOfWork
{
    Task<ActionResponse<IEnumerable<MarkModel>>> ComboAsync(string email, Guid id);

    Task<ActionResponse<IEnumerable<MarkModel>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<MarkModel>> GetAsync(Guid id);

    Task<ActionResponse<MarkModel>> UpdateAsync(MarkModel modelo);

    Task<ActionResponse<MarkModel>> AddAsync(MarkModel modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}