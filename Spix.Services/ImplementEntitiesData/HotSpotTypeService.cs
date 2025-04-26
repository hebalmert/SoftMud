using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Spix.Core.EntitiesData;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Helper.Extensions;
using Spix.Helper.Helpers;
using Spix.Helper.Transactions;
using Spix.Infrastructure;
using Spix.Services.InterfacesEntitiesData;

namespace Spix.Services.ImplementEntitiesData;

public class HotSpotTypeService : IHotSpotTypeService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITransactionManager _transactionManager;
    private readonly HttpErrorHandler _httpErrorHandler;

    public HotSpotTypeService(DataContext context, IHttpContextAccessor httpContextAccessor,
        ITransactionManager transactionManager)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _transactionManager = transactionManager;
        _httpErrorHandler = new HttpErrorHandler();
    }

    public async Task<ActionResponse<IEnumerable<HotSpotType>>> ComboAsync()
    {
        try
        {
            var ListModel = await _context.HotSpotTypes.Where(x => x.Active).ToListAsync();

            return new ActionResponse<IEnumerable<HotSpotType>>
            {
                WasSuccess = true,
                Result = ListModel
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<HotSpotType>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<IEnumerable<HotSpotType>>> GetAsync(PaginationDTO pagination)
    {
        try
        {
            var queryable = _context.HotSpotTypes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.TypeName!.ToLower().Contains(pagination.Filter.ToLower()));
            }

            await _httpContextAccessor.HttpContext!.InsertParameterPagination(queryable, pagination.RecordsNumber);
            var modelo = await queryable.OrderBy(x => x.TypeName).Paginate(pagination).ToListAsync();

            return new ActionResponse<IEnumerable<HotSpotType>>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<HotSpotType>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<HotSpotType>> GetAsync(int id)
    {
        try
        {
            var modelo = await _context.HotSpotTypes.FindAsync(id);
            if (modelo == null)
            {
                return new ActionResponse<HotSpotType>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            return new ActionResponse<HotSpotType>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<HotSpotType>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<HotSpotType>> UpdateAsync(HotSpotType modelo)
    {
        await _transactionManager.BeginTransactionAsync();

        try
        {
            _context.HotSpotTypes.Update(modelo);

            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<HotSpotType>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<HotSpotType>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<HotSpotType>> AddAsync(HotSpotType modelo)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            _context.HotSpotTypes.Add(modelo);
            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<HotSpotType>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<HotSpotType>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<bool>> DeleteAsync(int id)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var DataRemove = await _context.HotSpotTypes.FindAsync(id);
            if (DataRemove == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            _context.HotSpotTypes.Remove(DataRemove);

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