using Spix.Core.EntitiesOper;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesOper;

public interface IContractorUnitOfWork
{
    Task<ActionResponse<IEnumerable<Contractor>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Contractor>> GetAsync(Guid id);

    Task<ActionResponse<Contractor>> UpdateAsync(Contractor modelo, string frontUrl);

    Task<ActionResponse<Contractor>> AddAsync(Contractor modelo, string email, string frontUrl);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}