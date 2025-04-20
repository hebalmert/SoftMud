using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.Entities;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesEntities;

namespace Spix.AppBack.Controllers.EntitiesV2;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/countries")]
[ApiController]
public class CountriesController : ControllerBase
{
    private readonly ICountriesUnitOfWork _countriesUnitOfWork;

    public CountriesController(ICountriesUnitOfWork countriesUnitOfWork)
    {
        _countriesUnitOfWork = countriesUnitOfWork;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetAll([FromQuery] PaginationDTO pagination)
    {
        var response = await _countriesUnitOfWork.GetAsync2(pagination);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }
}