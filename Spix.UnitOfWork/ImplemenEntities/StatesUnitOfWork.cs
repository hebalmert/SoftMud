using Spix.Core.Entities;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntities;
using Spix.UnitOfWork.InterfacesEntities;

namespace Spix.UnitOfWork.ImplemenEntities;

public class StatesUnitOfWork : IStatesUnitOfWork
{
    private readonly IStatesService _statesService;

    public StatesUnitOfWork(IStatesService statesService)
    {
        _statesService = statesService;
    }

    public async Task<ActionResponse<IEnumerable<State>>> ComboAsync(int id) => await _statesService.ComboAsync(id);

    public async Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination) => await _statesService.GetAsync(pagination);

    public async Task<ActionResponse<State>> GetAsync(int id) => await _statesService.GetAsync(id);

    public async Task<ActionResponse<State>> UpdateAsync(State modelo) => await _statesService.UpdateAsync(modelo);

    public async Task<ActionResponse<State>> AddAsync(State modelo) => await _statesService.AddAsync(modelo);

    public async Task<ActionResponse<bool>> DeleteAsync(int id) => await _statesService.DeleteAsync(id);
}