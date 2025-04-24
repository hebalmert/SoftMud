using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.Entities;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesEntities;

namespace Spix.AppBack.Controllers.EntitiesV1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/managers")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ApiController]
    public class ManagersController : ControllerBase
    {
        private readonly IManagerUnitOfWork _managerUnitOfWork;
        private readonly IConfiguration _configuration;

        public ManagersController(IManagerUnitOfWork managerUnitOfWork, IConfiguration configuration)
        {
            _managerUnitOfWork = managerUnitOfWork;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Manager>>> GetAll([FromQuery] PaginationDTO pagination)
        {
            var response = await _managerUnitOfWork.GetAsync(pagination);
            if (!response.WasSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _managerUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpPut]
        public async Task<ActionResult<Manager>> PutAsync(Manager modelo)
        {
            var response = await _managerUnitOfWork.UpdateAsync(modelo, _configuration["UrlFrontend"]!);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpPost]
        public async Task<ActionResult<Manager>> PostAsync(Manager modelo)
        {
            //string baseUrl = $"{Request.Scheme}://{Request.Host}";
            //string frontUrl = _configuration["UrlFrontend"]!;
            var response = await _managerUnitOfWork.AddAsync(modelo, _configuration["UrlFrontend"]!);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            var response = await _managerUnitOfWork.DeleteAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }
    }
}