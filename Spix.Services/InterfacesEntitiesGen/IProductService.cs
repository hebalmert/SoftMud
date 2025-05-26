using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesEntitiesGen;

public interface IProductService
{
    Task<ActionResponse<IEnumerable<Product>>> ComboAsync(string email, Guid id);

    Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Product>> GetAsync(Guid id);

    Task<ActionResponse<Product>> UpdateAsync(Product modelo);

    Task<ActionResponse<Product>> AddAsync(Product modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}