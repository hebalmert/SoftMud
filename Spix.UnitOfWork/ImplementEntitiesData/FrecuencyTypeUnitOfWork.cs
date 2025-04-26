using Spix.Core.EntitiesData;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesData;
using Spix.UnitOfWork.InterfacesEntitiesData;

namespace Spix.UnitOfWork.ImplementEntitiesData;

public class FrecuencyTypeUnitOfWork : IFrecuencyTypeUnitOfWork
{
    private readonly IFrecuencyTypeService _frecuencyTypeService;

    public FrecuencyTypeUnitOfWork(IFrecuencyTypeService frecuencyTypeService)
    {
        _frecuencyTypeService = frecuencyTypeService;
    }

    public async Task<ActionResponse<IEnumerable<FrecuencyType>>> ComboAsync() => await _frecuencyTypeService.ComboAsync();

    public async Task<ActionResponse<IEnumerable<FrecuencyType>>> GetAsync(PaginationDTO pagination) => await _frecuencyTypeService.GetAsync(pagination);

    public async Task<ActionResponse<FrecuencyType>> GetAsync(int id) => await _frecuencyTypeService.GetAsync(id);

    public async Task<ActionResponse<FrecuencyType>> UpdateAsync(FrecuencyType modelo) => await _frecuencyTypeService.UpdateAsync(modelo);

    public async Task<ActionResponse<FrecuencyType>> AddAsync(FrecuencyType modelo) => await _frecuencyTypeService.AddAsync(modelo);

    public async Task<ActionResponse<bool>> DeleteAsync(int id) => await _frecuencyTypeService.DeleteAsync(id);
}