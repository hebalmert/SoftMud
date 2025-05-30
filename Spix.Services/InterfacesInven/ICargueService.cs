using Spix.Core.EntitiesInven;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesInven;

public interface ICargueService
{
    Task<ActionResponse<IEnumerable<EnumItemModel>>> GetComboStatus();

    Task<ActionResponse<IEnumerable<Cargue>>> GetAsync(PaginationDTO pagination, string email);

    Task<ActionResponse<Cargue>> GetAsync(Guid id);

    Task<ActionResponse<Cargue>> UpdateAsync(Cargue modelo);

    Task<ActionResponse<Cargue>> AddAsync(Cargue modelo, string email);

    Task<ActionResponse<bool>> DeleteAsync(Guid id);
}