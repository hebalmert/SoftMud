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

public class ProductService : IProductService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITransactionManager _transactionManager;
    private readonly IUserHelper _userHelper;
    private readonly HttpErrorHandler _httpErrorHandler;

    public ProductService(DataContext context, IHttpContextAccessor httpContextAccessor,
        ITransactionManager transactionManager, IUserHelper userHelper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _transactionManager = transactionManager;
        _userHelper = userHelper;
        _httpErrorHandler = new HttpErrorHandler();
    }

    public async Task<ActionResponse<IEnumerable<Product>>> GetAsync(PaginationDTO pagination, string email)
    {
        try
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<Product>>
                {
                    WasSuccess = false,
                    Message = "Problemas de Validacion de Usuario"
                };
            }

            var queryable = _context.Products.Where(x => x.CorporationId == user.CorporationId && x.ProductCategoryId == pagination.GuidId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.ProductName!.ToLower().Contains(pagination.Filter.ToLower()));
            }

            await _httpContextAccessor.HttpContext!.InsertParameterPagination(queryable, pagination.RecordsNumber);
            var modelo = await queryable.OrderBy(x => x.ProductName).Paginate(pagination).ToListAsync();

            return new ActionResponse<IEnumerable<Product>>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<Product>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Product>> GetAsync(Guid id)
    {
        try
        {
            var modelo = await _context.Products.FindAsync(id);
            if (modelo == null)
            {
                return new ActionResponse<Product>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            return new ActionResponse<Product>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<Product>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Product>> UpdateAsync(Product modelo)
    {
        await _transactionManager.BeginTransactionAsync();

        try
        {
            _context.Products.Update(modelo);

            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<Product>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Product>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Product>> AddAsync(Product modelo, string email)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<Product>
                {
                    WasSuccess = false,
                    Message = "Problemas de Validacion de Usuario"
                };
            }
            modelo.CorporationId = Convert.ToInt32(user.CorporationId);
            _context.Products.Add(modelo);
            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<Product>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Product>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var DataRemove = await _context.Products.FindAsync(id);
            if (DataRemove == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            _context.Products.Remove(DataRemove);

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