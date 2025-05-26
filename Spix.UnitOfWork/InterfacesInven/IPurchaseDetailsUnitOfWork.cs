using Spix.Core.EntitiesInven;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesInven;

public interface IPurchaseDetailsUnitOfWork
{
    Task<ActionResponse<IEnumerable<EnumItemModel>>> GetComboStatus();

    Task<ActionResponse<IEnumerable<PurchaseDetail>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<PurchaseDetail>> GetAsync(Guid id);

    Task<ActionResponse<PurchaseDetail>> UpdateAsync(PurchaseDetail modelo);

    Task<ActionResponse<PurchaseDetail>> AddAsync(PurchaseDetail modelo, string email);

    Task<ActionResponse<Purchase>> ClosePurchaseSync(Purchase modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}