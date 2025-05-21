using Spix.Core.EntitiesOper;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesOper;
using Spix.UnitOfWork.InterfacesOper;

namespace Spix.UnitOfWork.ImplementOper;

public class ContractorUnitOfWork : IContractorUnitOfWork
{
    private readonly IContractorService _contractorService;

    public ContractorUnitOfWork(IContractorService contractorService)
    {
        _contractorService = contractorService;
    }

    public async Task<ActionResponse<IEnumerable<Contractor>>> ComboAsync(string email) => await _contractorService.ComboAsync(email);

    public async Task<ActionResponse<IEnumerable<Contractor>>> GetAsync(PaginationDTO pagination, string email) => await _contractorService.GetAsync(pagination, email);

    public async Task<ActionResponse<Contractor>> GetAsync(Guid id) => await _contractorService.GetAsync(id);

    public async Task<ActionResponse<Contractor>> UpdateAsync(Contractor modelo, string frontUrl) => await _contractorService.UpdateAsync(modelo, frontUrl);

    public async Task<ActionResponse<Contractor>> AddAsync(Contractor modelo, string email, string frontUrl) => await _contractorService.AddAsync(modelo, email, frontUrl);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _contractorService.DeleteAsync(id);
}