using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Spix.Core.Entities;
using Spix.Core.EntitiesData;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Helper.Extensions;
using Spix.Helper.Helpers;
using Spix.Helper.Transactions;
using Spix.Infrastructure;
using Spix.Services.InterfacesEntitiesData;

namespace Spix.Services.ImplementEntitiesData;

public class FrecuencyService : IFrecuencyService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITransactionManager _transactionManager;
    private readonly HttpErrorHandler _httpErrorHandler;

    public FrecuencyService(DataContext context, IHttpContextAccessor httpContextAccessor,
        ITransactionManager transactionManager, IMemoryCache cache)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _transactionManager = transactionManager;
        _httpErrorHandler = new HttpErrorHandler();
    }

    public async Task<ActionResponse<IEnumerable<Frecuency>>> ComboAsync(int id)
    {
        try
        {
            var ListModel = await _context.Frecuencies.Where(x => x.FrecuencyTypeId == id).ToListAsync();

            return new ActionResponse<IEnumerable<Frecuency>>
            {
                WasSuccess = true,
                Result = ListModel
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<Frecuency>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<IEnumerable<Frecuency>>> GetAsync(PaginationDTO pagination)
    {
        try
        {
            var queryable = _context.Frecuencies.Where(x => x.FrecuencyTypeId == pagination.Id).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.FrecuencyName!.ToString().Contains(pagination.Filter));
            }

            await _httpContextAccessor.HttpContext!.InsertParameterPagination(queryable, pagination.RecordsNumber);
            var modelo = await queryable.OrderBy(x => x.FrecuencyName).Paginate(pagination).ToListAsync();

            return new ActionResponse<IEnumerable<Frecuency>>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<Frecuency>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Frecuency>> GetAsync(int id)
    {
        try
        {
            var modelo = await _context.Frecuencies.FindAsync(id);
            if (modelo == null)
            {
                return new ActionResponse<Frecuency>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            return new ActionResponse<Frecuency>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<Frecuency>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Frecuency>> UpdateAsync(Frecuency modelo)
    {
        await _transactionManager.BeginTransactionAsync();

        try
        {
            _context.Frecuencies.Update(modelo);

            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<Frecuency>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Frecuency>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Frecuency>> AddAsync(Frecuency modelo)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            _context.Frecuencies.Add(modelo);
            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<Frecuency>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Frecuency>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<bool>> DeleteAsync(int id)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var DataRemove = await _context.Frecuencies.FindAsync(id);
            if (DataRemove == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            _context.Frecuencies.Remove(DataRemove);

            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

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