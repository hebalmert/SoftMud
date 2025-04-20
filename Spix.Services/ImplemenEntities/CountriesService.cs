using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Spix.Core.Entities;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Helper.Extensions;
using Spix.Helper.Helpers;
using Spix.Helper.Transactions;
using Spix.Infrastructure;
using Spix.Services.InterfacesEntities;

namespace Spix.Services.ImplemenEntities;

public class CountriesService : ICountriesService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITransactionManager _transactionManager;
    private readonly HttpErrorHandler _httpErrorHandler;
    private readonly IMemoryCache _cache;
    // 🔹 Variables centralizadas para nombres de caché

    private readonly string _cacheComboList;
    private readonly string _cacheList;
    private readonly string _cacheModelo;

    public CountriesService(DataContext context, IHttpContextAccessor httpContextAccessor,
        ITransactionManager transactionManager, IMemoryCache cache)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _transactionManager = transactionManager;
        _cache = cache;
        _httpErrorHandler = new HttpErrorHandler();
        // ✅ Inicialización de claves de caché en el constructor

        _cacheComboList = "States_Combo_List";
        _cacheList = "States_List";
        _cacheModelo = "State_";
    }

    private string GetCacheKeyForModelo(int id) => $"{_cacheModelo}{id}";

    private void ClearCacheList()
    {
        // Elimina la caché global y cualquier variante de `_cacheList`
        _cache.Remove(_cacheList);

        var cacheKeys = _cache.Get<List<string>>("Country_Keys");
        if (cacheKeys != null)
        {
            foreach (var key in cacheKeys)
            {
                _cache.Remove(key); // Borra cada variante paginada
            }
            _cache.Remove("Country_Keys"); // Borra la lista de claves
        }
    }

    private void ClearCacheForModelo(int id)
    {
        _cache.Remove(GetCacheKeyForModelo(id));
        ClearCacheList();
        _cache.Remove(_cacheComboList);
    }

    public async Task<ActionResponse<IEnumerable<Country>>> ComboAsync()
    {
        // Verificar si los países ya están en el caché
        string cacheKey = $"{_cacheComboList}";
        if (_cache.TryGetValue(cacheKey, out IEnumerable<Country>? cachedModelo))
        {
            return new ActionResponse<IEnumerable<Country>> { WasSuccess = true, Result = cachedModelo };
        }

        try
        {
            var ListModel = await _context.Countries.ToListAsync();

            // Guardar los datos en caché con una expiración de 10 minutos
            _cache.Set(cacheKey, ListModel, TimeSpan.FromDays(1));

            return new ActionResponse<IEnumerable<Country>>
            {
                WasSuccess = true,
                Result = ListModel
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<Country>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination)
    {
        string cacheKey = $"{_cacheList}_{pagination.Page}_{pagination.RecordsNumber}_{pagination.Filter}";
        if (_cache.TryGetValue(cacheKey, out IEnumerable<Country>? cachedModelo))
        {
            return new ActionResponse<IEnumerable<Country>> { WasSuccess = true, Result = cachedModelo };
        }

        try
        {
            var queryable = _context.Countries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name!.ToLower().Contains(pagination.Filter.ToLower()));
            }

            await _httpContextAccessor.HttpContext!.InsertParameterPagination(queryable, pagination.RecordsNumber);
            var modelo = await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync();

            ////Para el manejo de Cache
            _cache.Set(cacheKey, modelo, TimeSpan.FromDays(1)); // Guarda el caché con clave específica

            // Guardar la clave de caché para eliminación futura
            List<string> cacheKeys = _cache.Get<List<string>>("Country_Keys") ?? new List<string>();
            cacheKeys.Add(cacheKey);
            _cache.Set("Country_Keys", cacheKeys, TimeSpan.FromDays(1));

            return new ActionResponse<IEnumerable<Country>>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<Country>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<IEnumerable<Country>>> GetAsync2(PaginationDTO pagination)
    {
        try
        {
            var queryable = _context.Countries.Include(x => x.States).AsQueryable();

            await _httpContextAccessor.HttpContext!.InsertParameterPagination(queryable, pagination.RecordsNumber);
            var countries = await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync();

            return new ActionResponse<IEnumerable<Country>>
            {
                WasSuccess = true,
                Result = countries
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<Country>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Country>> GetAsync(int id)
    {
        string cacheKey = GetCacheKeyForModelo(id);
        if (_cache.TryGetValue(cacheKey, out Country? cachedModelo))
        {
            return new ActionResponse<Country> { WasSuccess = true, Result = cachedModelo };
        }

        try
        {
            var modelo = await _context.Countries.FindAsync(id);
            if (modelo == null)
            {
                return new ActionResponse<Country>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            //Para el manejo de Cache
            _cache.Set(cacheKey, modelo, TimeSpan.FromDays(1)); // ✅ Guarda en caché

            return new ActionResponse<Country>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<Country>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Country>> UpdateAsync(Country modelo)
    {
        await _transactionManager.BeginTransactionAsync();

        try
        {
            _context.Countries.Update(modelo);

            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            //Para el manejo de Cache
            ClearCacheForModelo(modelo.CountryId);

            var updatedModelo = await _context.SoftPlans.ToListAsync();
            _cache.Set(_cacheComboList, updatedModelo, TimeSpan.FromDays(1));
            _cache.Set(GetCacheKeyForModelo(modelo.CountryId), modelo, TimeSpan.FromDays(10));

            return new ActionResponse<Country>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Country>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Country>> AddAsync(Country modelo)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            _context.Countries.Add(modelo);
            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            //Para el manejo de Cache
            ClearCacheForModelo(modelo.CountryId);

            var updatedModelo = await _context.SoftPlans.ToListAsync();
            _cache.Set(_cacheComboList, updatedModelo, TimeSpan.FromDays(1));
            _cache.Set(GetCacheKeyForModelo(modelo.CountryId), modelo, TimeSpan.FromDays(10));

            return new ActionResponse<Country>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Country>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<bool>> DeleteAsync(int id)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var DataRemove = await _context.Countries.FindAsync(id);
            if (DataRemove == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            _context.Countries.Remove(DataRemove);

            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            //Para el manejo de Cache
            ClearCacheForModelo(id);

            var updatedModelo = await _context.SoftPlans.ToListAsync();
            _cache.Set(_cacheComboList, updatedModelo, TimeSpan.FromDays(1));

            return new ActionResponse<bool>
            {
                WasSuccess = true,
                Result = true
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<bool>(ex); // ✅ Manejo de errores automático
        }
    }
}