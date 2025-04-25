using Spix.Core.EntitiesData;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesData;
using Spix.UnitOfWork.InterfacesEntitiesData;

namespace Spix.UnitOfWork.ImplementEntitiesData;

public class SecurityUnitOfWork : ISecurityUnitOfWork
{
    private readonly ISecurityService _securityService;

    public SecurityUnitOfWork(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<ActionResponse<IEnumerable<Security>>> ComboAsync() => await _securityService.ComboAsync();

    public async Task<ActionResponse<IEnumerable<Security>>> GetAsync(PaginationDTO pagination) => await _securityService.GetAsync(pagination);

    public async Task<ActionResponse<Security>> GetAsync(int id) => await _securityService.GetAsync(id);

    public async Task<ActionResponse<Security>> UpdateAsync(Security modelo) => await _securityService.UpdateAsync(modelo);

    public async Task<ActionResponse<Security>> AddAsync(Security modelo) => await _securityService.AddAsync(modelo);

    public async Task<ActionResponse<bool>> DeleteAsync(int id) => await _securityService.DeleteAsync(id);
}