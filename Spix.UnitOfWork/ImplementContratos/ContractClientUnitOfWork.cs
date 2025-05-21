using Spix.Core.EntitiesContratos;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesContratos;
using Spix.UnitOfWork.InterfaceContratos;

namespace Spix.UnitOfWork.ImplementContratos;

public class ContractClientUnitOfWork : IContractClientUnitOfWork
{
    private readonly IContractClientService _contractClientService;

    public ContractClientUnitOfWork(IContractClientService contractClientService)
    {
        _contractClientService = contractClientService;
    }

    public async Task<ActionResponse<IEnumerable<ContractClient>>> GetControlContratos(PaginationDTO pagination, string email) => await _contractClientService.GetControlContratos(pagination, email);

    public async Task<ActionResponse<IEnumerable<ContractClient>>> GetAsync(PaginationDTO pagination, string email) => await _contractClientService.GetAsync(pagination, email);

    public async Task<ActionResponse<ContractClient>> GetAsync(Guid id) => await _contractClientService.GetAsync(id);

    public async Task<ActionResponse<ContractClient>> GetProcesandoAsync(Guid id) => await _contractClientService.GetProcesandoAsync(id);

    public async Task<ActionResponse<ContractClient>> UpdateAsync(ContractClient modelo) => await _contractClientService.UpdateAsync(modelo);

    public async Task<ActionResponse<ContractClient>> AddAsync(ContractClient modelo, string email) => await _contractClientService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _contractClientService.DeleteAsync(id);
}