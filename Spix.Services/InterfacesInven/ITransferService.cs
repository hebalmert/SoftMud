using Spix.Core.EntitiesInven;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesInven;

public interface ITransferService
{
    Task<ActionResponse<IEnumerable<EnumItemModel>>> GetComboStatus();

    Task<ActionResponse<IEnumerable<Transfer>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Transfer>> GetAsync(Guid id);

    Task<ActionResponse<Transfer>> UpdateAsync(Transfer modelo);

    Task<ActionResponse<Transfer>> AddAsync(Transfer modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}