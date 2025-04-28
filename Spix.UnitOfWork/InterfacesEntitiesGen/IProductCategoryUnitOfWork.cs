using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesEntitiesGen;

public interface IProductCategoryUnitOfWork
{
    Task<ActionResponse<IEnumerable<ProductCategory>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<ProductCategory>> GetAsync(Guid id);

    Task<ActionResponse<ProductCategory>> UpdateAsync(ProductCategory modelo);

    Task<ActionResponse<ProductCategory>> AddAsync(ProductCategory modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}