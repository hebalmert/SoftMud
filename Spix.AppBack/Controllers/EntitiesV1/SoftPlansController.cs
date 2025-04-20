using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.Entities;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesEntities;

namespace Spix.AppBack.Controllers.EntitiesV1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/softplans")]
[ApiController]
public class SoftPlansController : ControllerBase
{
    private readonly ISoftPlanUnitOfWork _softPlanUnitOfWork;

    public SoftPlansController(ISoftPlanUnitOfWork softPlanUnitOfWork)
    {
        _softPlanUnitOfWork = softPlanUnitOfWork;
    }

    [HttpGet("loadCombo")]
    public async Task<ActionResult<IEnumerable<State>>> GetComboAsync()
    {
        var response = await _softPlanUnitOfWork.ComboAsync();
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<State>>> GetAll([FromQuery] PaginationDTO pagination)
    {
        var response = await _softPlanUnitOfWork.GetAsync(pagination);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var response = await _softPlanUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpPut]
    public async Task<ActionResult<State>> PutAsync(SoftPlan modelo)
    {
        var response = await _softPlanUnitOfWork.UpdateAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpPost]
    public async Task<ActionResult<State>> PostAsync(SoftPlan modelo)
    {
        var response = await _softPlanUnitOfWork.AddAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var response = await _softPlanUnitOfWork.DeleteAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }
}