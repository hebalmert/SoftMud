using Spix.Core.EntitiesInven;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.ReportsDTO;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesInven;
using Spix.UnitOfWork.InterfacesInven;

namespace Spix.UnitOfWork.ImplementInven;

public class PurchaseUnitOfWork : IPurchaseUnitOfWork
{
    private readonly IPurchaseService _purchaseService;

    public PurchaseUnitOfWork(IPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    public async Task<ActionResponse<IEnumerable<EnumItemModel>>> GetComboStatus() => await _purchaseService.GetComboStatus();

    public async Task<ActionResponse<IEnumerable<Purchase>>> GetReporteSellDates(ReportDataDTO pagination, string email) => await _purchaseService.GetReporteSellDates(pagination, email);

    public async Task<ActionResponse<IEnumerable<Purchase>>> GetAsync(PaginationDTO pagination, string email) => await _purchaseService.GetAsync(pagination, email);

    public async Task<ActionResponse<Purchase>> GetAsync(Guid id) => await _purchaseService.GetAsync(id);

    public async Task<ActionResponse<Purchase>> UpdateAsync(Purchase modelo) => await _purchaseService.UpdateAsync(modelo);

    public async Task<ActionResponse<Purchase>> AddAsync(Purchase modelo, string email) => await _purchaseService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _purchaseService.DeleteAsync(id);
}