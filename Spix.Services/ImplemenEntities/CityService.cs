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

public class CityService : ICityService
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

    public CityService(DataContext context, IHttpContextAccessor httpContextAccessor,
        ITransactionManager transactionManager, IMemoryCache cache)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _transactionManager = transactionManager;
        _cache = cache;
        _httpErrorHandler = new HttpErrorHandler();
        // ✅ Inicialización de claves de caché en el constructor

        _cacheComboList = "Cities_Combo_List";
        _cacheList = "Cities_List";
        _cacheModelo = "Cities_";
    }

    private string GetCacheKeyForModelo(int id) => $"{_cacheModelo}{id}";

    private void ClearCacheList()
    {
        // Elimina la caché global y cualquier variante de `_cacheList`
        _cache.Remove(_cacheList);

        var cacheKeys = _cache.Get<List<string>>("City_Keys");
        if (cacheKeys != null)
        {
            foreach (var key in cacheKeys)
            {
                _cache.Remove(key); // Borra cada variante paginada
            }
            _cache.Remove("City_Keys"); // Borra la lista de claves
        }
    }

    private void ClearCacheForModelo(int id)
    {
        _cache.Remove(GetCacheKeyForModelo(id));
        ClearCacheList();
        _cache.Remove(_cacheComboList);
    }

    public async Task<ActionResponse<IEnumerable<City>>> ComboAsync(int id)
    {
        // Verificar si los países ya están en el caché
        string cacheKey = $"{_cacheComboList}";
        if (_cache.TryGetValue(cacheKey, out IEnumerable<City>? cachedModelo))
        {
            return new ActionResponse<IEnumerable<City>> { WasSuccess = true, Result = cachedModelo };
        }

        try
        {
            var ListModel = await _context.Cities.Where(x => x.StateId == id).ToListAsync();

            // Guardar los datos en caché con una expiración de 10 minutos
            _cache.Set(cacheKey, ListModel, TimeSpan.FromDays(1));

            return new ActionResponse<IEnumerable<City>>
            {
                WasSuccess = true,
                Result = ListModel
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<City>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<IEnumerable<City>>> GetAsync(PaginationDTO pagination)
    {
        string cacheKey = $"{_cacheList}_{pagination.Page}_{pagination.RecordsNumber}_{pagination.Filter}";

        if (_cache.TryGetValue(cacheKey, out IEnumerable<City>? cachedModelo))
        {
            return new ActionResponse<IEnumerable<City>> { WasSuccess = true, Result = cachedModelo };
        }

        try
        {
            var queryable = _context.Cities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name!.ToLower().Contains(pagination.Filter.ToLower()));
            }

            await _httpContextAccessor.HttpContext!.InsertParameterPagination(queryable, pagination.RecordsNumber);
            var modelo = await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync();

            ////Para el manejo de Cache
            _cache.Set(cacheKey, modelo, TimeSpan.FromDays(1)); // Guarda el caché con clave específica

            // Guardar la clave de caché para eliminación futura
            List<string> cacheKeys = _cache.Get<List<string>>("City_Keys") ?? new List<string>();
            cacheKeys.Add(cacheKey);
            _cache.Set("City_Keys", cacheKeys, TimeSpan.FromDays(1));

            return new ActionResponse<IEnumerable<City>>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<City>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<City>> GetAsync(int id)
    {
        string cacheKey = GetCacheKeyForModelo(id);
        if (_cache.TryGetValue(cacheKey, out City? cachedModelo))
        {
            return new ActionResponse<City> { WasSuccess = true, Result = cachedModelo };
        }

        try
        {
            var modelo = await _context.Cities.FindAsync(id);
            if (modelo == null)
            {
                return new ActionResponse<City>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            //Para el manejo de Cache
            _cache.Set(cacheKey, modelo, TimeSpan.FromDays(1)); // ✅ Guarda en caché

            return new ActionResponse<City>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<City>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<City>> UpdateAsync(City modelo)
    {
        await _transactionManager.BeginTransactionAsync();

        try
        {
            _context.Cities.Update(modelo);

            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            //Para el manejo de Cache
            ClearCacheForModelo(modelo.StateId);

            var updatedModelo = await _context.Cities.Where(x=> x.StateId == modelo.StateId).ToListAsync();
            _cache.Set(_cacheComboList, updatedModelo, TimeSpan.FromDays(1));
            _cache.Set(GetCacheKeyForModelo(modelo.CityId), modelo, TimeSpan.FromDays(10));

            return new ActionResponse<City>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<City>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<City>> AddAsync(City modelo)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            _context.Cities.Add(modelo);
            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            //Para el manejo de Cache
            ClearCacheForModelo(modelo.StateId);

            var updatedModelo = await _context.Cities.Where(x => x.StateId == modelo.StateId).ToListAsync();
            _cache.Set(_cacheComboList, updatedModelo, TimeSpan.FromDays(1));
            _cache.Set(GetCacheKeyForModelo(modelo.CityId), modelo, TimeSpan.FromDays(10));

            return new ActionResponse<City>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<City>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<bool>> DeleteAsync(int id)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var DataRemove = await _context.Cities.FindAsync(id);
            if (DataRemove == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            _context.Cities.Remove(DataRemove);

            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            //Para el manejo de Cache
            ClearCacheForModelo(id);

            var updatedModelo = await _context.Cities.Where(x=> x.StateId == DataRemove.StateId).ToListAsync();
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