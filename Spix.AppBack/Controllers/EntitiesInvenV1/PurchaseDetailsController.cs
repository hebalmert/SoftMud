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
    [Route("api/v{version:apiVersion}/purchaseDetails")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]
    [ApiController]
    public class PurchaseDetailsController : ControllerBase
    {
        private readonly IPurchaseDetailsUnitOfWork _purchaseDetailsUnitOfWork;
        private readonly IConfiguration _configuration;

        public PurchaseDetailsController(IPurchaseDetailsUnitOfWork purchaseDetailsUnitOfWork, IConfiguration configuration)
        {
            _purchaseDetailsUnitOfWork = purchaseDetailsUnitOfWork;
            _configuration = configuration;
        }

        [HttpGet("loadComboStatus")]
        public async Task<ActionResult<IEnumerable<EnumItemModel>>> GetCombo()
        {
            var response = await _purchaseDetailsUnitOfWork.GetComboStatus();
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PurchaseDetail>>> GetAll([FromQuery] PaginationDTO pagination)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            if (email == null)
            {
                return BadRequest("Erro en el sistema de Usuarios");
            }

            var response = await _purchaseDetailsUnitOfWork.GetAsync(pagination, email);
            if (!response.WasSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var response = await _purchaseDetailsUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpPut]
        public async Task<ActionResult<PurchaseDetail>> PutAsync(PurchaseDetail modelo)
        {
            var response = await _purchaseDetailsUnitOfWork.UpdateAsync(modelo);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpPost]
        public async Task<ActionResult<PurchaseDetail>> PostAsync(PurchaseDetail modelo)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            if (email == null)
            {
                return BadRequest("Erro en el sistema de Usuarios");
            }

            var response = await _purchaseDetailsUnitOfWork.AddAsync(modelo, email);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpPost("CerrarPurchase")]
        public async Task<ActionResult<Purchase>> PostClosePurchaseAsync(Purchase modelo)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            if (email == null)
            {
                return BadRequest("Erro en el sistema de Usuarios");
            }

            var response = await _purchaseDetailsUnitOfWork.ClosePurchaseSync(modelo, email);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync(Guid id)
        {
            var response = await _purchaseDetailsUnitOfWork.DeleteAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }
    }
}