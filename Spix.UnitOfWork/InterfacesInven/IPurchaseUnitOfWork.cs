using Spix.Core.EntitiesInven;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.ReportsDTO;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesInven;

public interface IPurchaseUnitOfWork
{
    Task<ActionResponse<IEnumerable<EnumItemModel>>> GetComboStatus();

    Task<ActionResponse<IEnumerable<Purchase>>> GetReporteSellDates(ReportDataDTO pagination, string email);

    Task<ActionResponse<IEnumerable<Purchase>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Purchase>> GetAsync(Guid id);

    Task<ActionResponse<Purchase>> UpdateAsync(Purchase modelo);

    Task<ActionResponse<Purchase>> AddAsync(Purchase modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}