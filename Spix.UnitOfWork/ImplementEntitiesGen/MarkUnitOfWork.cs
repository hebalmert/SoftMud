using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesGen;
using Spix.UnitOfWork.InterfacesEntitiesGen;

namespace Spix.UnitOfWork.ImplementEntitiesGen;

public class MarkUnitOfWork : IMarkUnitOfWork
{
    private readonly IMarkService _markService;

    public MarkUnitOfWork(IMarkService markService)
    {
        _markService = markService;
    }

    public async Task<ActionResponse<IEnumerable<Mark>>> ComboAsync(string email) => await _markService.ComboAsync(email);

    public async Task<ActionResponse<IEnumerable<Mark>>> GetAsync(PaginationDTO pagination, string email) => await _markService.GetAsync(pagination, email);

    public async Task<ActionResponse<Mark>> GetAsync(Guid id) => await _markService.GetAsync(id);

    public async Task<ActionResponse<Mark>> UpdateAsync(Mark modelo) => await _markService.UpdateAsync(modelo);

    public async Task<ActionResponse<Mark>> AddAsync(Mark modelo, string email) => await _markService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _markService.DeleteAsync(id);
}