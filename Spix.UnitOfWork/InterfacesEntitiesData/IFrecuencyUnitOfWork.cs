using Spix.Core.EntitiesData;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesEntitiesData;

public interface IFrecuencyUnitOfWork
{
    Task<ActionResponse<IEnumerable<Frecuency>>> ComboAsync(int id);

    Task<ActionResponse<IEnumerable<Frecuency>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<Frecuency>> GetAsync(int id);

    Task<ActionResponse<Frecuency>> UpdateAsync(Frecuency modelo);

    Task<ActionResponse<Frecuency>> AddAsync(Frecuency modelo);

    Task<ActionResponse<bool>> DeleteAsync(int id);
}