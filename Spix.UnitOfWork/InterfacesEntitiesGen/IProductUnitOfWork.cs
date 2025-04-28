using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesEntitiesGen;

public interface IProductUnitOfWork
{
    Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Product>> GetAsync(Guid id);

    Task<ActionResponse<Product>> UpdateAsync(Product modelo);

    Task<ActionResponse<Product>> AddAsync(Product modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}