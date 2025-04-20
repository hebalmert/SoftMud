using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Spix.Core.Entities;
using Spix.CoreShared.Pagination;
using Spix.UnitOfWork.InterfacesEntities;

namespace Spix.AppBack.Controllers.EntitiesV1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/states")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly IStatesUnitOfWork _statesUnitOfWork;

        public StatesController(IStatesUnitOfWork statesUnitOfWork)
        {
            _statesUnitOfWork = statesUnitOfWork;
        }

        [HttpGet("loadCombo/{id:int}")]
        public async Task<ActionResult<IEnumerable<State>>> GetComboAsync(int id)
        {
            var response = await _statesUnitOfWork.ComboAsync(id);
            if (!response.WasSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetAll([FromQuery] PaginationDTO pagination)
        {
            var response = await _statesUnitOfWork.GetAsync(pagination);
            if (!response.WasSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var response = await _statesUnitOfWork.GetAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpPut]
        public async Task<ActionResult<State>> PutAsync(State modelo)
        {
            var response = await _statesUnitOfWork.UpdateAsync(modelo);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpPost]
        public async Task<ActionResult<State>> PostAsync(State modelo)
        {
            var response = await _statesUnitOfWork.AddAsync(modelo);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync(int id)
        {
            var response = await _statesUnitOfWork.DeleteAsync(id);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return NotFound(response.Message);
        }
    }
}