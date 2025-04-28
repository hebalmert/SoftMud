using Microsoft.AspNetCore.Http;
using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Helper.Helpers;
using Spix.Helper.Transactions;
using Spix.Helper;
using Spix.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Spix.Helper.Extensions;
using Spix.Services.InterfacesEntitiesGen;

namespace Spix.Services.ImplementEntitiesGen;

public class TaxService : ITaxService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITransactionManager _transactionManager;
    private readonly IUserHelper _userHelper;
    private readonly HttpErrorHandler _httpErrorHandler;

    public TaxService(DataContext context, IHttpContextAccessor httpContextAccessor,
        ITransactionManager transactionManager, IUserHelper userHelper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _transactionManager = transactionManager;
        _userHelper = userHelper;
        _httpErrorHandler = new HttpErrorHandler();
    }

    public async Task<ActionResponse<IEnumerable<Tax>>> ComboAsync(string email)
    {
        try
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<Tax>>
                {
                    WasSuccess = false,
                    Message = "Problemas de Validacion de Usuario"
                };
            }
            var ListModel = await _context.Taxes.Where(x => x.Active && x.CorporationId == user.CorporationId).ToListAsync();

            return new ActionResponse<IEnumerable<Tax>>
            {
                WasSuccess = true,
                Result = ListModel
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<Tax>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<IEnumerable<Tax>>> GetAsync(PaginationDTO pagination, string email)
    {
        try
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<Tax>>
                {
                    WasSuccess = false,
                    Message = "Problemas de Validacion de Usuario"
                };
            }

            var queryable = _context.Taxes.Where(x => x.CorporationId == user.CorporationId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.TaxName!.ToLower().Contains(pagination.Filter.ToLower()));
            }

            await _httpContextAccessor.HttpContext!.InsertParameterPagination(queryable, pagination.RecordsNumber);
            var modelo = await queryable.OrderBy(x => x.TaxName).Paginate(pagination).ToListAsync();

            return new ActionResponse<IEnumerable<Tax>>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<Tax>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Tax>> GetAsync(Guid id)
    {
        try
        {
            var modelo = await _context.Taxes.FindAsync(id);
            if (modelo == null)
            {
                return new ActionResponse<Tax>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            return new ActionResponse<Tax>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<Tax>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Tax>> UpdateAsync(Tax modelo)
    {
        await _transactionManager.BeginTransactionAsync();

        try
        {
            _context.Taxes.Update(modelo);

            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<Tax>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Tax>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Tax>> AddAsync(Tax modelo, string email)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<Tax>
                {
                    WasSuccess = false,
                    Message = "Problemas de Validacion de Usuario"
                };
            }
            modelo.CorporationId = Convert.ToInt32(user.CorporationId);
            _context.Taxes.Add(modelo);
            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<Tax>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Tax>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var DataRemove = await _context.Taxes.FindAsync(id);
            if (DataRemove == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            _context.Taxes.Remove(DataRemove);

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