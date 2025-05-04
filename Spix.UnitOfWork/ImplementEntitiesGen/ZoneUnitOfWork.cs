using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntitiesGen;
using Spix.UnitOfWork.InterfacesEntitiesGen;

namespace Spix.UnitOfWork.ImplementEntitiesGen;

public class ZoneUnitOfWork : IZoneUnitOfWork
{
    private readonly IZoneService _zoneService;

    public ZoneUnitOfWork(IZoneService zoneService)
    {
        _zoneService = zoneService;
    }

    public async Task<ActionResponse<IEnumerable<Zone>>> ComboAsync(string email, int id) => await _zoneService.ComboAsync(email, id);

    public async Task<ActionResponse<IEnumerable<Zone>>> GetAsync(PaginationDTO pagination, string email) => await _zoneService.GetAsync(pagination, email);

    public async Task<ActionResponse<Zone>> GetAsync(Guid id) => await _zoneService.GetAsync(id);

    public async Task<ActionResponse<Zone>> UpdateAsync(Zone modelo) => await _zoneService.UpdateAsync(modelo);

    public async Task<ActionResponse<Zone>> AddAsync(Zone modelo, string email) => await _zoneService.AddAsync(modelo, email);

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id) => await _zoneService.DeleteAsync(id);
}