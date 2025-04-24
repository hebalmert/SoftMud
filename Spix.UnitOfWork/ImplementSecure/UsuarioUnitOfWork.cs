using Spix.Core.EntitesSoftSec;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesSecure;
using Spix.UnitOfWork.InterfacesSecure;

namespace Spix.UnitOfWork.ImplementSecure;

public class UsuarioUnitOfWork : IUsuarioUnitOfWork
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioUnitOfWork(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    public async Task<ActionResponse<IEnumerable<Usuario>>> GetAsync(PaginationDTO pagination, string Email) => await _usuarioService.GetAsync(pagination, Email);

    public async Task<ActionResponse<Usuario>> GetAsync(int id) => await _usuarioService.GetAsync(id);

    public async Task<ActionResponse<Usuario>> UpdateAsync(Usuario modelo, string urlFront) => await _usuarioService.UpdateAsync(modelo, urlFront);

    public async Task<ActionResponse<Usuario>> AddAsync(Usuario modelo, string urlFront, string Email) => await _usuarioService.AddAsync(modelo, urlFront, Email);

    public async Task<ActionResponse<bool>> DeleteAsync(int id) => await _usuarioService.DeleteAsync(id);
}