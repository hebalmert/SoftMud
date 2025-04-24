using Spix.Core.EntitesSoftSec;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesSecure;

namespace Spix.UnitOfWork.ImplementSecure;

public class UsuarioRoleUnitOfWork
{
    private readonly IUsuarioRoleService _usuarioRoleService;

    public UsuarioRoleUnitOfWork(IUsuarioRoleService usuarioRoleService)
    {
        _usuarioRoleService = usuarioRoleService;
    }

    public async Task<ActionResponse<IEnumerable<EnumItemModel>>> ComboAsync() => await _usuarioRoleService.ComboAsync();

    public async Task<ActionResponse<IEnumerable<UsuarioRole>>> GetAsync(PaginationDTO pagination) => await _usuarioRoleService.GetAsync(pagination);

    public async Task<ActionResponse<UsuarioRole>> GetAsync(int id) => await _usuarioRoleService.GetAsync(id);

    public async Task<ActionResponse<UsuarioRole>> AddAsync(UsuarioRole modelo, string Email) => await _usuarioRoleService.AddAsync(modelo, Email);

    public async Task<ActionResponse<bool>> DeleteAsync(int id) => await _usuarioRoleService.DeleteAsync(id);
}