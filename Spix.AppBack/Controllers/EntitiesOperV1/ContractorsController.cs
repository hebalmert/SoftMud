using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.EntitiesGen;
using Spix.Core.EntitiesOper;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesOper;
using System.Security.Claims;

namespace Spix.AppBack.Controllers.EntitiesV1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/contractor")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]
    [ApiController]
    public class ContractorsController : ControllerBase
    {
        private readonly IContractorUnitOfWork _contractorUnitOfWork;
        private readonly IConfiguration _configuration;

        public ContractorsController(IContractorUnitOfWork contractorUnitOfWork, IConfiguration configuration)
        {
            _contractorUnitOfWork = contractorUnitOfWork;
            _configuration = configuration;
        }

        [HttpGet("loadCombo")]
        public async Task<ActionResult<IEnumerable<Zone>>> GetComboAsync(int id)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            if (email == null)
            {
                return BadRequest("Erro en el sistema de Usuarios");
            }

            var response = await _contractorUnitOfWork.ComboAsync(email);
            if (!response.WasSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contractor>>> GetAll([FromQuery] PaginationDTO pagination)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            if (email == null)
            {
                return BadRequest("Erro en el sistema de Usuarios");
            }

            var response = await _contractorUnitOfWork.GetAsync(pagination, email);
            if (!response.WasSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var response = await _contractorUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpPut]
        public async Task<ActionResult<Contractor>> PutAsync(Contractor modelo)
        {
            var response = await _contractorUnitOfWork.UpdateAsync(modelo, _configuration["UrlFrontend"]!);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpPost]
        public async Task<ActionResult<Contractor>> PostAsync(Contractor modelo)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            if (email == null)
            {
                return BadRequest("Erro en el sistema de Usuarios");
            }

            var response = await _contractorUnitOfWork.AddAsync(modelo, email, _configuration["UrlFrontend"]!);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync(Guid id)
        {
            var response = await _contractorUnitOfWork.DeleteAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }
    }
}