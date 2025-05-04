using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.UnitOfWork.InterfacesEntitiesGen;

public interface IZoneUnitOfWork
{
    Task<ActionResponse<IEnumerable<Zone>>> ComboAsync(string email, int id);

    Task<ActionResponse<IEnumerable<Zone>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Zone>> GetAsync(Guid id);

    Task<ActionResponse<Zone>> UpdateAsync(Zone modelo);

    Task<ActionResponse<Zone>> AddAsync(Zone modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}