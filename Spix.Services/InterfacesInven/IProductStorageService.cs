using Spix.Core.EntitiesGen;
using Spix.Core.EntitiesInven;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesInven;

public interface IProductStorageService
{
    Task<ActionResponse<IEnumerable<ProductStorage>>> ComboAsync(string email);

    Task<ActionResponse<IEnumerable<ProductStorage>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<ProductStorage>> GetAsync(Guid id);

    Task<ActionResponse<ProductStorage>> UpdateAsync(ProductStorage modelo);

    Task<ActionResponse<ProductStorage>> AddAsync(ProductStorage modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}