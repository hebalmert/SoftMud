﻿using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.EntitiesData;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesEntitiesData;

namespace Spix.AppBack.Controllers.EntitiesDataV1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/operations")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
[ApiController]
public class OperationsController : ControllerBase
{
    private readonly IOperationUnitOfWork _operationUnitOfWork;

    public OperationsController(IOperationUnitOfWork operationUnitOfWork)
    {
        _operationUnitOfWork = operationUnitOfWork;
    }

    [HttpGet("loadCombo")]
    public async Task<ActionResult<IEnumerable<Operation>>> GetComboAsync()
    {
        var response = await _operationUnitOfWork.ComboAsync();
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Operation>>> GetAll([FromQuery] PaginationDTO pagination)
    {
        var response = await _operationUnitOfWork.GetAsync(pagination);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var response = await _operationUnitOfWork.GetAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpPut]
    public async Task<ActionResult<Operation>> PutAsync(Operation modelo)
    {
        var response = await _operationUnitOfWork.UpdateAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpPost]
    public async Task<ActionResult<Operation>> PostAsync(Operation modelo)
    {
        var response = await _operationUnitOfWork.AddAsync(modelo);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteAsync(int id)
    {
        var response = await _operationUnitOfWork.DeleteAsync(id);
        if (response.WasSuccess)
        {
            return Ok(response.Result);
        }
        return NotFound(response.Message);
    }
}