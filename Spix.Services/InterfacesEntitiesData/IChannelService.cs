﻿using Spix.Core.EntitiesData;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;

namespace Spix.Services.InterfacesEntitiesData;

public interface IChannelService
{
    Task<ActionResponse<IEnumerable<Channel>>> ComboAsync();

    Task<ActionResponse<IEnumerable<Channel>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<Channel>> GetAsync(int id);

    Task<ActionResponse<Channel>> UpdateAsync(Channel modelo);

    Task<ActionResponse<Channel>> AddAsync(Channel modelo);

    Task<ActionResponse<bool>> DeleteAsync(int id);
}