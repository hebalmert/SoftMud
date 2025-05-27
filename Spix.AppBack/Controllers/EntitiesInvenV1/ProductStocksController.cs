using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.EntitiesInven;
using Spix.CoreShared.EntitiesDTO;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesInven;
using System.Security.Claims;

namespace Spix.AppBack.Controllers.EntitiesInvenV1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/productStocks")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Usuario")]
    [ApiController]
    public class ProductStocksController : ControllerBase
    {
        private readonly IProductStockUnitOfWork _productStockUnitOfWork;
        private readonly IConfiguration _configuration;

        public ProductStocksController(IProductStockUnitOfWork productStockUnitOfWork, IConfiguration configuration)
        {
            _productStockUnitOfWork = productStockUnitOfWork;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductStock>>> GetAll([FromQuery] PaginationDTO pagination)
        {
            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)!.Value;
            if (email == null)
            {
                return BadRequest("Erro en el sistema de Usuarios");
            }

            var response = await _productStockUnitOfWork.GetAsync(pagination, email);
            if (!response.WasSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var response = await _productStockUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }

        [HttpGet("transferstock")]
        public async Task<IActionResult> GetProductStock([FromQuery] TransferStockDTO modelo)
        {
            var response = await _productStockUnitOfWork.GetProductStock(modelo);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest(response.Message);
        }
    }
}