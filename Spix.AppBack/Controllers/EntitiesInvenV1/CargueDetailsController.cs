using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.EntitiesInven;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesInven;
using System.Security.Claims;

namespace Spix.AppBack.Controllers.EntitiesGenV1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/cargueDetails")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]
[ApiController]
public class CargueDetailsController : ControllerBase
{
    private readonly ICargueDetailsUnitOfWork _cargueDetailsUnitOfWork;

    public CargueDetailsController(ICargueDetailsUnitOfWork cargueDetailsUnitOfWork)
    {
        _cargueDetailsUnitOfWork = cargueDetailsUnitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CargueDetail>>> GetAll([FromQuery] PaginationDTO pagination)
    {
        string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        if (email == null)
        {
            return BadRequest("Erro en el sistema de Usuarios");
        }

        var response = await _cargueDetailsUnitOfWork.GetAsync(pagination, email);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet("GetSerials")]
    public async Task<ActionResult<IEnumerable<CargueDetail>>> GetSerialsAll([FromQuery] PaginationDTO pagination)
    {
        string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        if (email == null)
        {
            return BadRequest("Erro en el sistema de Usuarios");
        }

        var response = await _cargueDetailsUnitOfWork.GetSerialsAsync(pagination, email);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var response = await _cargueDetailsUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpPut]
    public async Task<ActionResult<CargueDetail>> PutAsync(CargueDetail modelo)
    {
        var response = await _cargueDetailsUnitOfWork.UpdateAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpPost]
    public async Task<ActionResult<CargueDetail>> PostAsync(CargueDetail modelo)
    {
        string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        if (email == null)
        {
            return BadRequest("Erro en el sistema de Usuarios");
        }

        var response = await _cargueDetailsUnitOfWork.AddAsync(modelo, email);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpGet("CerrarTrans/{id}")]
    public async Task<ActionResult<Cargue>> PostCerrarTransAsyncAsync(Guid id)
    {
        string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        if (email == null)
        {
            return BadRequest("Erro en el sistema de Usuarios");
        }

        var response = await _cargueDetailsUnitOfWork.CerrarCargueAsync(id, email);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(Guid id)
    {
        var response = await _cargueDetailsUnitOfWork.DeleteAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }
}