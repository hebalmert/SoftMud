using Spix.Core.EntitiesInven;
using Spix.CoreShared.EntitiesDTO;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesInven;
using Spix.UnitOfWork.InterfacesInven;

namespace Spix.UnitOfWork.ImplementInven;

public class ProductStockUnitOfWork : IProductStockUnitOfWork
{
    private readonly IProductStockService _productStockService;

    public ProductStockUnitOfWork(IProductStockService productStockService)
    {
        _productStockService = productStockService;
    }

    public async Task<ActionResponse<IEnumerable<ProductStock>>> GetAsync(PaginationDTO pagination, string email) => await _productStockService.GetAsync(pagination, email);

    public async Task<ActionResponse<ProductStock>> GetAsync(Guid id) => await _productStockService.GetAsync(id);

    public async Task<ActionResponse<TransferStockDTO>> GetProductStock(TransferStockDTO modelo) => await _productStockService.GetProductStock(modelo);
}