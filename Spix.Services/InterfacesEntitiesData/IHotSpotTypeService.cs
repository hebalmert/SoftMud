﻿using Spix.Core.EntitiesData;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesEntitiesData;

public interface IHotSpotTypeService
{
    Task<ActionResponse<IEnumerable<HotSpotType>>> ComboAsync();

    Task<ActionResponse<IEnumerable<HotSpotType>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<HotSpotType>> GetAsync(int id);

    Task<ActionResponse<HotSpotType>> UpdateAsync(HotSpotType modelo);

    Task<ActionResponse<HotSpotType>> AddAsync(HotSpotType modelo);

    Task<ActionResponse<bool>> DeleteAsync(int id);
}