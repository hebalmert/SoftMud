using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.Entities;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesEntities;

namespace Spix.AppBack.Controllers.EntitiesV1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/countries")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly ICountriesUnitOfWork _countriesUnitOfWork;

    public CountriesController(ICountriesUnitOfWork countriesUnitOfWork)
    {
        _countriesUnitOfWork = countriesUnitOfWork;
    }

    [HttpGet("loadCombo")]
    public async Task<ActionResult<IEnumerable<Country>>> GetComboAsync()
    {
        var response = await _countriesUnitOfWork.ComboAsync();
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetAll([FromQuery] PaginationDTO pagination)
    {
        var response = await _countriesUnitOfWork.GetAsync(pagination);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var response = await _countriesUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpPut]
    public async Task<ActionResult<Country>> PutAsync(Country modelo)
    {
        var response = await _countriesUnitOfWork.UpdateAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpPost]
    public async Task<ActionResult<Country>> PostAsync(Country modelo)
    {
        var response = await _countriesUnitOfWork.AddAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var response = await _countriesUnitOfWork.DeleteAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }
}