using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.EntitiesContratos;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfaceContratos;
using System.Security.Claims;

namespace Spix.AppBack.Controllers.EntitiesContractV1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/contractclients")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]
    [ApiController]
    public class ContractClientsController : ControllerBase
    {
        private readonly IContractClientUnitOfWork _contractClientUnitOfWork;
        private readonly IConfiguration _configuration;

        public ContractClientsController(IContractClientUnitOfWork contractClientUnitOfWork, IConfiguration configuration)
        {
            _contractClientUnitOfWork = contractClientUnitOfWork;
            _configuration = configuration;
        }

        [HttpGet("procesando/{id}")]
        public async Task<IActionResult> GetProcesandoAsync(Guid id)
        {
            var response = await _contractClientUnitOfWork.GetProcesandoAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpGet("contractControl")]
        public async Task<ActionResult<IEnumerable<ContractClient>>> GetControlContratos([FromQuery] PaginationDTO pagination)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            if (email == null)
            {
                return BadRequest("Erro en el sistema de Usuarios");
            }

            var response = await _contractClientUnitOfWork.GetControlContratos(pagination, email);
            if (!response.WasSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContractClient>>> GetAll([FromQuery] PaginationDTO pagination)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            if (email == null)
            {
                return BadRequest("Erro en el sistema de Usuarios");
            }

            var response = await _contractClientUnitOfWork.GetAsync(pagination, email);
            if (!response.WasSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var response = await _contractClientUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpPut]
        public async Task<ActionResult<ContractClient>> PutAsync(ContractClient modelo)
        {
            var response = await _contractClientUnitOfWork.UpdateAsync(modelo);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpPost]
        public async Task<ActionResult<ContractClient>> PostAsync(ContractClient modelo)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            if (email == null)
            {
                return BadRequest("Erro en el sistema de Usuarios");
            }

            var response = await _contractClientUnitOfWork.AddAsync(modelo, email);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync(Guid id)
        {
            var response = await _contractClientUnitOfWork.DeleteAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }
    }
}