using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.EntitesSoftSec;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesSecure;
using System.Security.Claims;

namespace Spix.AppBack.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/usuarioRoles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]
    [ApiController]
    public class UsuariosRoleController : ControllerBase
    {
        private readonly IUsuarioRoleUnitOfWork _usuarioRoleUnitOfWork;

        public UsuariosRoleController(IUsuarioRoleUnitOfWork usuarioRoleUnitOfWork)
        {
            _usuarioRoleUnitOfWork = usuarioRoleUnitOfWork;
        }

        [HttpGet("loadCombo")]
        public async Task<ActionResult<IEnumerable<EnumItemModel>>> GetComboAsync()
        {
            var response = await _usuarioRoleUnitOfWork.ComboAsync();
            if (!response.WasSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioRole>>> GetAll([FromQuery] PaginationDTO pagination)
        {
            var response = await _usuarioRoleUnitOfWork.GetAsync(pagination);
            if (!response.WasSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _usuarioRoleUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioRole>> PostAsync(UsuarioRole modelo)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;

            var response = await _usuarioRoleUnitOfWork.AddAsync(modelo, email);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            var response = await _usuarioRoleUnitOfWork.DeleteAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }
    }
}