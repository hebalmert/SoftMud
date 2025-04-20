﻿using Spix.Core.Entities;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Services.InterfacesEntities;
using Spix.UnitOfWork.InterfacesEntities;

namespace Spix.UnitOfWork.ImplemenEntities;

public class CountriesUnitOfWork : ICountriesUnitOfWork
{
    private readonly ICountriesService _countriesService;

    public CountriesUnitOfWork(ICountriesService countriesService)
    {
        _countriesService = countriesService;
    }

    public async Task<ActionResponse<IEnumerable<Country>>> ComboAsync() => await _countriesService.ComboAsync();

    public async Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination) => await _countriesService.GetAsync(pagination);

    public async Task<ActionResponse<IEnumerable<Country>>> GetAsync2(PaginationDTO pagination) => await _countriesService.GetAsync2(pagination);

    public async Task<ActionResponse<Country>> GetAsync(int id) => await _countriesService.GetAsync(id);

    public async Task<ActionResponse<Country>> UpdateAsync(Country modelo) => await _countriesService.UpdateAsync(modelo);

    public async Task<ActionResponse<Country>> AddAsync(Country modelo) => await _countriesService.AddAsync(modelo);

    public async Task<ActionResponse<bool>> DeleteAsync(int id) => await _countriesService.DeleteAsync(id);
}