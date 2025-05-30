using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.EntitiesInven;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesInven;
using System.Security.Claims;

namespace Spix.AppBack.Controllers.EntitiesGenV1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/cargues")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]
[ApiController]
public class CarguesController : ControllerBase
{
    private readonly ICargueUnitOfWork _cargueUnitOfWork;

    public CarguesController(ICargueUnitOfWork cargueUnitOfWork)
    {
        _cargueUnitOfWork = cargueUnitOfWork;
    }

    [HttpGet("loadComboStatus")]
    public async Task<ActionResult<IEnumerable<EnumItemModel>>> GetCombo()
    {
        var response = await _cargueUnitOfWork.GetComboStatus();
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cargue>>> GetAll([FromQuery] PaginationDTO pagination)
    {
        string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        if (email == null)
        {
            return BadRequest("Erro en el sistema de Usuarios");
        }

        var response = await _cargueUnitOfWork.GetAsync(pagination, email);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var response = await _cargueUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpPut]
    public async Task<ActionResult<Cargue>> PutAsync(Cargue modelo)
    {
        var response = await _cargueUnitOfWork.UpdateAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpPost]
    public async Task<ActionResult<Cargue>> PostAsync(Cargue modelo)
    {
        string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        if (email == null)
        {
            return BadRequest("Erro en el sistema de Usuarios");
        }

        var response = await _cargueUnitOfWork.AddAsync(modelo, email);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(Guid id)
    {
        var response = await _cargueUnitOfWork.DeleteAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }
}