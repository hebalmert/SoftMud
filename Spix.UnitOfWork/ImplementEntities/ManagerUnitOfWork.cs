using Spix.Core.Entities;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntities;

namespace Spix.UnitOfWork.ImplementEntities;

public class ManagerUnitOfWork : IManagerService
{
    private readonly IManagerService _managerService;

    public ManagerUnitOfWork(IManagerService managerService)
    {
        _managerService = managerService;
    }

    public async Task<ActionResponse<IEnumerable<Manager>>> GetAsync(PaginationDTO pagination) => await _managerService.GetAsync(pagination);

    public async Task<ActionResponse<Manager>> GetAsync(int id) => await _managerService.GetAsync(id);

    public async Task<ActionResponse<Manager>> UpdateAsync(Manager modelo, string frontUrl) => await _managerService.UpdateAsync(modelo, frontUrl);

    public async Task<ActionResponse<Manager>> AddAsync(Manager modelo, string frontUrl) => await _managerService.AddAsync(modelo, frontUrl);

    public async Task<ActionResponse<bool>> DeleteAsync(int id) => await _managerService.DeleteAsync(id);
}