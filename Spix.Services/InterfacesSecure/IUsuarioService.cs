using Spix.Core.EntitesSoftSec;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesSecure;

public interface IUsuarioService
{
    Task<ActionResponse<IEnumerable<Usuario>>> GetAsync(PaginationDTO pagination, string Email);

    Task<ActionResponse<Usuario>> GetAsync(int id);

    Task<ActionResponse<Usuario>> UpdateAsync(Usuario modelo, string urlFront);

    Task<ActionResponse<Usuario>> AddAsync(Usuario modelo, string urlFront, string Email);

    Task<ActionResponse<bool>> DeleteAsync(int id);
}