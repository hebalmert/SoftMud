using Microsoft.AspNetCore.Http;
using Spix.Core.EntitiesGen;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.Helper.Helpers;
using Spix.Helper.Transactions;
using Spix.Helper;
using Spix.Infrastructure;
using Spix.Services.InterfacesEntitiesGen;
using Spix.Helper.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Spix.Services.ImplementEntitiesGen;

public class RegisterService : IRegisterService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITransactionManager _transactionManager;
    private readonly IUserHelper _userHelper;
    private readonly HttpErrorHandler _httpErrorHandler;

    public RegisterService(DataContext context, IHttpContextAccessor httpContextAccessor,
        ITransactionManager transactionManager, IUserHelper userHelper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _transactionManager = transactionManager;
        _userHelper = userHelper;
        _httpErrorHandler = new HttpErrorHandler();
    }

    public async Task<ActionResponse<IEnumerable<Register>>> GetAsync(PaginationDTO pagination, string email)
    {
        try
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<Register>>
                {
                    WasSuccess = false,
                    Message = "Problemas de Validacion de Usuario"
                };
            }

            var queryable = _context.Registers.Where(x => x.CorporationId == user.CorporationId).AsQueryable();

            await _httpContextAccessor.HttpContext!.InsertParameterPagination(queryable, pagination.RecordsNumber);
            var modelo = await queryable.Paginate(pagination).ToListAsync();

            return new ActionResponse<IEnumerable<Register>>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<Register>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Register>> GetAsync(Guid id)
    {
        try
        {
            var modelo = await _context.Registers.FindAsync(id);
            if (modelo == null)
            {
                return new ActionResponse<Register>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            return new ActionResponse<Register>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<Register>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Register>> UpdateAsync(Register modelo)
    {
        await _transactionManager.BeginTransactionAsync();

        try
        {
            _context.Registers.Update(modelo);

            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<Register>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Register>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Register>> AddAsync(Register modelo, string email)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<Register>
                {
                    WasSuccess = false,
                    Message = "Problemas de Validacion de Usuario"
                };
            }
            modelo.CorporationId = Convert.ToInt32(user.CorporationId);
            _context.Registers.Add(modelo);
            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<Register>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Register>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var DataRemove = await _context.Registers.FindAsync(id);
            if (DataRemove == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            _context.Registers.Remove(DataRemove);

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