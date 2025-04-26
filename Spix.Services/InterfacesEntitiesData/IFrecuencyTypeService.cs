using Spix.Core.EntitiesData;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesEntitiesData;

public interface IFrecuencyTypeService
{
    Task<ActionResponse<IEnumerable<FrecuencyType>>> ComboAsync();

    Task<ActionResponse<IEnumerable<FrecuencyType>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<FrecuencyType>> GetAsync(int id);

    Task<ActionResponse<FrecuencyType>> UpdateAsync(FrecuencyType modelo);

    Task<ActionResponse<FrecuencyType>> AddAsync(FrecuencyType modelo);

    Task<ActionResponse<bool>> DeleteAsync(int id);
}