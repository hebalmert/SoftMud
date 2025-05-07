using Spix.Core.EntitiesOper;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesOper;
using Spix.UnitOfWork.InterfacesOper;

namespace Spix.UnitOfWork.ImplementOper;

public class ClientUnitOfWork : IClientUnitOfWork
{
    private readonly IClientService _clientService;

    public ClientUnitOfWork(IClientService clientService)
    {
        _clientService = clientService;
    }

    public async Task<ActionResponse<IEnumerable<Client>>> GetAsync(PaginationDTO pagination, string email) => await _clientService.GetAsync(pagination, email);

    public async Task<ActionResponse<Client>> GetAsync(Guid id) => await _clientService.GetAsync(id);

    public async Task<ActionResponse<Client>> UpdateAsync(Client modelo, string frontUrl) => await _clientService.UpdateAsync(modelo, frontUrl);

    public async Task<ActionResponse<Client>> AddAsync(Client modelo, string email, string frontUrl) => await _clientService.AddAsync(modelo, email, frontUrl);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _clientService.DeleteAsync(id);
}