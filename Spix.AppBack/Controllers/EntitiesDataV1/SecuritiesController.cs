using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.EntitiesData;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesEntitiesData;

namespace Spix.AppBack.Controllers.EntitiesDataV1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/securities")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
[ApiController]
public class SecuritiesController : ControllerBase
{
    private readonly ISecurityUnitOfWork _securityUnitOfWork;

    public SecuritiesController(ISecurityUnitOfWork securityUnitOfWork)
    {
        _securityUnitOfWork = securityUnitOfWork;
    }

    [HttpGet("loadCombo")]
    public async Task<ActionResult<IEnumerable<Security>>> GetComboAsync()
    {
        var response = await _securityUnitOfWork.ComboAsync();
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Security>>> GetAll([FromQuery] PaginationDTO pagination)
    {
        var response = await _securityUnitOfWork.GetAsync(pagination);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var response = await _securityUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpPut]
    public async Task<ActionResult<Security>> PutAsync(Security modelo)
    {
        var response = await _securityUnitOfWork.UpdateAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpPost]
    public async Task<ActionResult<Security>> PostAsync(Security modelo)
    {
        var response = await _securityUnitOfWork.AddAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var response = await _securityUnitOfWork.DeleteAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }
}