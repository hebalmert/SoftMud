using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesGen;
using Spix.UnitOfWork.InterfacesEntitiesGen;

namespace Spix.UnitOfWork.ImplementEntitiesGen;

public class TaxUnitOfWork : ITaxUnitOfWork
{
    private readonly ITaxService _taxService;

    public TaxUnitOfWork(ITaxService taxService)
    {
        _taxService = taxService;
    }

    public async Task<ActionResponse<IEnumerable<Tax>>> ComboAsync(string email) => await _taxService.ComboAsync(email);

    public async Task<ActionResponse<IEnumerable<Tax>>> GetAsync(PaginationDTO pagination, string email) => await _taxService.GetAsync(pagination, email);

    public async Task<ActionResponse<Tax>> GetAsync(Guid id) => await _taxService.GetAsync(id);

    public async Task<ActionResponse<Tax>> UpdateAsync(Tax modelo) => await _taxService.UpdateAsync(modelo);

    public async Task<ActionResponse<Tax>> AddAsync(Tax modelo, string email) => await _taxService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _taxService.DeleteAsync(id);
}