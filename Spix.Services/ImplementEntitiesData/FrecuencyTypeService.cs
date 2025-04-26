using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

public class FrecuencyTypeService : IFrecuencyTypeService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITransactionManager _transactionManager;
    private readonly HttpErrorHandler _httpErrorHandler;

    public FrecuencyTypeService(DataContext context, IHttpContextAccessor httpContextAccessor,
        ITransactionManager transactionManager)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _transactionManager = transactionManager;
        _httpErrorHandler = new HttpErrorHandler();
    }

    public async Task<ActionResponse<IEnumerable<FrecuencyType>>> ComboAsync()
    {
        try
        {
            var ListModel = await _context.FrecuencyTypes.Where(x => x.Active).ToListAsync();

            return new ActionResponse<IEnumerable<FrecuencyType>>
            {
                WasSuccess = true,
                Result = ListModel
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<FrecuencyType>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<IEnumerable<FrecuencyType>>> GetAsync(PaginationDTO pagination)
    {
        try
        {
            var queryable = _context.FrecuencyTypes.Include(x => x.Frecuencies).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.TypeName!.ToLower().Contains(pagination.Filter.ToLower()));
            }

            await _httpContextAccessor.HttpContext!.InsertParameterPagination(queryable, pagination.RecordsNumber);
            var modelo = await queryable.OrderBy(x => x.TypeName).Paginate(pagination).ToListAsync();

            return new ActionResponse<IEnumerable<FrecuencyType>>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<FrecuencyType>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<FrecuencyType>> GetAsync(int id)
    {
        try
        {
            var modelo = await _context.FrecuencyTypes.FindAsync(id);
            if (modelo == null)
            {
                return new ActionResponse<FrecuencyType>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            return new ActionResponse<FrecuencyType>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<FrecuencyType>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<FrecuencyType>> UpdateAsync(FrecuencyType modelo)
    {
        await _transactionManager.BeginTransactionAsync();

        try
        {
            _context.FrecuencyTypes.Update(modelo);

            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<FrecuencyType>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<FrecuencyType>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<FrecuencyType>> AddAsync(FrecuencyType modelo)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            _context.FrecuencyTypes.Add(modelo);
            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<FrecuencyType>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<FrecuencyType>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<bool>> DeleteAsync(int id)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var DataRemove = await _context.FrecuencyTypes.FindAsync(id);
            if (DataRemove == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            _context.FrecuencyTypes.Remove(DataRemove);

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