using Spix.Core.EntitiesData;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesEntitiesData;

public interface IChainTypesService
{
    Task<ActionResponse<IEnumerable<ChainType>>> ComboAsync();

    Task<ActionResponse<IEnumerable<ChainType>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<ChainType>> GetAsync(int id);

    Task<ActionResponse<ChainType>> UpdateAsync(ChainType modelo);

    Task<ActionResponse<ChainType>> AddAsync(ChainType modelo);

    Task<ActionResponse<bool>> DeleteAsync(int id);
}