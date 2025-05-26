using Spix.Core.EntitiesInven;
using Spix.CoreShared.EntitiesDTO;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesInven;

public interface IProductStockUnitOfWork
{
    Task<ActionResponse<IEnumerable<ProductStock>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<ProductStock>> GetAsync(Guid id);

    Task<ActionResponse<TransferStockDTO>> GetProductStock(TransferStockDTO modelo);
}