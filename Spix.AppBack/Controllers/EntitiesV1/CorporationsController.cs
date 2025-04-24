using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.Entities;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesEntities;

namespace Spix.AppBack.Controllers.EntitiesV1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/corporations")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
[ApiController]
public class CorporationsController : ControllerBase
{
    private readonly ICorporationUnitOfWork _corporationUnitOfWork;

    public CorporationsController(ICorporationUnitOfWork corporationUnitOfWork)
    {
        _corporationUnitOfWork = corporationUnitOfWork;
    }

    [HttpGet("loadCombo")]
    public async Task<ActionResult<IEnumerable<Corporation>>> GetComboAsync()
    {
        var response = await _corporationUnitOfWork.ComboAsync();
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Corporation>>> GetAll([FromQuery] PaginationDTO pagination)
    {
        var response = await _corporationUnitOfWork.GetAsync(pagination);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var response = await _corporationUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpPut]
    public async Task<ActionResult<Corporation>> PutAsync(Corporation modelo)
    {
        var response = await _corporationUnitOfWork.UpdateAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpPost]
    public async Task<ActionResult<Corporation>> PostAsync(Corporation modelo)
    {
        var response = await _corporationUnitOfWork.AddAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var response = await _corporationUnitOfWork.DeleteAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }
}