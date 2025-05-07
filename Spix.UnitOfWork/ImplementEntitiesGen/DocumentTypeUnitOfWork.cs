using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesGen;
using Spix.UnitOfWork.InterfacesEntitiesGen;

namespace Spix.UnitOfWork.ImplementEntitiesGen;

public class DocumentTypeUnitOfWork : IDocumentTypeUnitOfWork
{
    private readonly IDocumentTypeService _documentTypeService;

    public DocumentTypeUnitOfWork(IDocumentTypeService documentTypeService)
    {
        _documentTypeService = documentTypeService;
    }

    public async Task<ActionResponse<IEnumerable<DocumentType>>> ComboAsync(string email) => await _documentTypeService.ComboAsync(email);

    public async Task<ActionResponse<IEnumerable<DocumentType>>> GetAsync(PaginationDTO pagination, string email) => await _documentTypeService.GetAsync(pagination, email);

    public async Task<ActionResponse<DocumentType>> GetAsync(Guid id) => await _documentTypeService.GetAsync(id);

    public async Task<ActionResponse<DocumentType>> UpdateAsync(DocumentType modelo) => await _documentTypeService.UpdateAsync(modelo);

    public async Task<ActionResponse<DocumentType>> AddAsync(DocumentType modelo, string email) => await _documentTypeService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _documentTypeService.DeleteAsync(id);
}