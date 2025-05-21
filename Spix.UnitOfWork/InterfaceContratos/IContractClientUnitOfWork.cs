using Spix.Core.EntitiesContratos;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfaceContratos;

public interface IContractClientUnitOfWork
{
    Task<ActionResponse<IEnumerable<ContractClient>>> GetControlContratos(PaginationDTO pagination, string email);

    Task<ActionResponse<IEnumerable<ContractClient>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<ContractClient>> GetAsync(Guid id);

    Task<ActionResponse<ContractClient>> GetProcesandoAsync(Guid id);

    Task<ActionResponse<ContractClient>> UpdateAsync(ContractClient modelo);

    Task<ActionResponse<ContractClient>> AddAsync(ContractClient modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}