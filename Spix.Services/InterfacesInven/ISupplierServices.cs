using Spix.Core.EntitiesInven;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesInven;

public interface ISupplierServices
{
    Task<ActionResponse<IEnumerable<Supplier>>> ComboAsync(string email);

    Task<ActionResponse<IEnumerable<Supplier>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Supplier>> GetAsync(Guid id);

    Task<ActionResponse<Supplier>> UpdateAsync(Supplier modelo, string frontUrl);

    Task<ActionResponse<Supplier>> AddAsync(Supplier modelo, string email, string frontUrl);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}