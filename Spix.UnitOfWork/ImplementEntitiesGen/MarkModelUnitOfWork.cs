using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesGen;
using Spix.UnitOfWork.InterfacesEntitiesGen;

namespace Spix.UnitOfWork.ImplementEntitiesGen;

public class MarkModelUnitOfWork : IMarkModelUnitOfWork
{
    private readonly IMarkModelService _markModelService;

    public MarkModelUnitOfWork(IMarkModelService markModelService)
    {
        _markModelService = markModelService;
    }

    public async Task<ActionResponse<IEnumerable<MarkModel>>> GetAsync(PaginationDTO pagination, string email) => await _markModelService.GetAsync(pagination, email);

    public async Task<ActionResponse<MarkModel>> GetAsync(Guid id) => await _markModelService.GetAsync(id);

    public async Task<ActionResponse<MarkModel>> UpdateAsync(MarkModel modelo) => await _markModelService.UpdateAsync(modelo);

    public async Task<ActionResponse<MarkModel>> AddAsync(MarkModel modelo, string email) => await _markModelService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _markModelService.DeleteAsync(id);
}