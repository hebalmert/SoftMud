using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.EntitiesInven;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesInven;
using System.Security.Claims;

namespace Spix.AppBack.Controllers.EntitiesInvenV1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/purchases")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseUnitOfWork _purchaseUnitOfWork;
        private readonly IConfiguration _configuration;

        public PurchasesController(IPurchaseUnitOfWork purchaseUnitOfWork, IConfiguration configuration)
        {
            _purchaseUnitOfWork = purchaseUnitOfWork;
            _configuration = configuration;
        }

        [HttpGet("loadComboStatus")]
        public async Task<ActionResult<IEnumerable<EnumItemModel>>> GetCombo()
        {
            var response = await _purchaseUnitOfWork.GetComboStatus();
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetAll([FromQuery] PaginationDTO pagination)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            if (email == null)
            {
                return BadRequest("Erro en el sistema de Usuarios");
            }

            var response = await _purchaseUnitOfWork.GetAsync(pagination, email);
            if (!response.WasSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var response = await _purchaseUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpPut]
        public async Task<ActionResult<Purchase>> PutAsync(Purchase modelo)
        {
            var response = await _purchaseUnitOfWork.UpdateAsync(modelo);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpPost]
        public async Task<ActionResult<Purchase>> PostAsync(Purchase modelo)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            if (email == null)
            {
                return BadRequest("Erro en el sistema de Usuarios");
            }

            var response = await _purchaseUnitOfWork.AddAsync(modelo, email);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync(Guid id)
        {
            var response = await _purchaseUnitOfWork.DeleteAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }
    }
}