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
[Route("api/v{version:apiVersion}/transfers")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]
[ApiController]
public class TransfersController : ControllerBase
{
    private readonly ITransferUnitOfWork _transferUnitOfWork;

    public TransfersController(ITransferUnitOfWork transferUnitOfWork)
    {
        _transferUnitOfWork = transferUnitOfWork;
    }

    [HttpGet("loadComboStatus")]
    public async Task<ActionResult<IEnumerable<EnumItemModel>>> GetCombo()
    {
        var response = await _transferUnitOfWork.GetComboStatus();
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transfer>>> GetAll([FromQuery] PaginationDTO pagination)
    {
        string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        if (email == null)
        {
            return BadRequest("Erro en el sistema de Usuarios");
        }

        var response = await _transferUnitOfWork.GetAsync(pagination, email);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        var response = await _transferUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpPut]
    public async Task<ActionResult<Transfer>> PutAsync(Transfer modelo)
    {
        var response = await _transferUnitOfWork.UpdateAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpPost]
    public async Task<ActionResult<Transfer>> PostAsync(Transfer modelo)
    {
        string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
        if (email == null)
        {
            return BadRequest("Erro en el sistema de Usuarios");
        }

        var response = await _transferUnitOfWork.AddAsync(modelo, email);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(Guid id)
    {
        var response = await _transferUnitOfWork.DeleteAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return BadRequest(response.Message);
    }
}