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
[Route("api/v{version:apiVersion}/transferDetails")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]
[ApiController]
public class TransferDetailsController : ControllerBase
{
    private readonly ITransferDetailsUnitOfWork _transferDetailsUnitOfWork;

    public TransferDetailsController(ITransferDetailsUnitOfWork transferDetailsUnitOfWork)
    {
        _transferDetailsUnitOfWork = transferDetailsUnitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransferDetails>>> GetAll([FromQuery] PaginationDTO pagination)
    {
        string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        if (email == null)
        {
            return BadRequest("Erro en el sistema de Usuarios");
        }

        var response = await _transferDetailsUnitOfWork.GetAsync(pagination, email);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var response = await _transferDetailsUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpPut]
    public async Task<ActionResult<TransferDetails>> PutAsync(TransferDetails modelo)
    {
        var response = await _transferDetailsUnitOfWork.UpdateAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpPost]
    public async Task<ActionResult<TransferDetails>> PostAsync(TransferDetails modelo)
    {
        string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        if (email == null)
        {
            return BadRequest("Erro en el sistema de Usuarios");
        }

        var response = await _transferDetailsUnitOfWork.AddAsync(modelo, email);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpPost("CerrarTrans")]
    public async Task<ActionResult<Transfer>> PostCerrarTransAsyncAsync(Transfer modelo)
    {
        string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        if (email == null)
        {
            return BadRequest("Erro en el sistema de Usuarios");
        }

        var response = await _transferDetailsUnitOfWork.CerrarTransAsync(modelo, email);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(Guid id)
    {
        var response = await _transferDetailsUnitOfWork.DeleteAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }
}