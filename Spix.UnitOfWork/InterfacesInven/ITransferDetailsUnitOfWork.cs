using Spix.Core.EntitiesGen;
using Spix.Core.EntitiesInven;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesInven;

public interface ITransferDetailsUnitOfWork
{
    Task<ActionResponse<IEnumerable<TransferDetails>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<TransferDetails>> GetAsync(Guid id);

    Task<ActionResponse<TransferDetails>> UpdateAsync(TransferDetails modelo);

    Task<ActionResponse<TransferDetails>> AddAsync(TransferDetails modelo, string email);

    Task<ActionResponse<Transfer>> CerrarTransAsync(Transfer modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}