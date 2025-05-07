using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesEntitiesGen;

public interface IDocumentTypeUnitOfWork
{
    Task<ActionResponse<IEnumerable<DocumentType>>> ComboAsync(string email);

    Task<ActionResponse<IEnumerable<DocumentType>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<DocumentType>> GetAsync(Guid id);

    Task<ActionResponse<DocumentType>> UpdateAsync(DocumentType modelo);

    Task<ActionResponse<DocumentType>> AddAsync(DocumentType modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}