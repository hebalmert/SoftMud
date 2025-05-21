using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesGen;
using Spix.UnitOfWork.InterfacesEntitiesGen;

namespace Spix.UnitOfWork.ImplementEntitiesGen;

public class RegisterUnitOfWork : IRegisterUnitOfWork
{
    private readonly IRegisterService _registerService;

    public RegisterUnitOfWork(IRegisterService registerService)
    {
        _registerService = registerService;
    }

    public async Task<ActionResponse<IEnumerable<Register>>> GetAsync(PaginationDTO pagination, string email) => await _registerService.GetAsync(pagination, email);

    public async Task<ActionResponse<Register>> GetAsync(Guid id) => await _registerService.GetAsync(id);

    public async Task<ActionResponse<Register>> UpdateAsync(Register modelo) => await _registerService.UpdateAsync(modelo);

    public async Task<ActionResponse<Register>> AddAsync(Register modelo, string email) => await _registerService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _registerService.DeleteAsync(id);
}