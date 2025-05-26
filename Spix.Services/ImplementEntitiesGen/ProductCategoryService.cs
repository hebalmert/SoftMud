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

public class ProductCategoryService : IProductCategoryService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITransactionManager _transactionManager;
    private readonly IUserHelper _userHelper;
    private readonly HttpErrorHandler _httpErrorHandler;

    public ProductCategoryService(DataContext context, IHttpContextAccessor httpContextAccessor,
        ITransactionManager transactionManager, IUserHelper userHelper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _transactionManager = transactionManager;
        _userHelper = userHelper;
        _httpErrorHandler = new HttpErrorHandler();
    }

    public async Task<ActionResponse<IEnumerable<ProductCategory>>> ComboAsync(string email)
    {
        try
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<ProductCategory>>
                {
                    WasSuccess = false,
                    Message = "Problemas de Validacion de Usuario"
                };
            }
            var ListModel = await _context.ProductCategories
                .Where(x => x.Active && x.CorporationId == user.CorporationId)
                .ToListAsync();

            return new ActionResponse<IEnumerable<ProductCategory>>
            {
                WasSuccess = true,
                Result = ListModel
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<ProductCategory>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<IEnumerable<ProductCategory>>> GetAsync(PaginationDTO pagination, string email)
    {
        try
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<ProductCategory>>
                {
                    WasSuccess = false,
                    Message = "Problemas de Validacion de Usuario"
                };
            }

            var queryable = _context.ProductCategories.Include(x => x.Products).Where(x => x.CorporationId == user.CorporationId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Name!.ToLower().Contains(pagination.Filter.ToLower()));
            }

            await _httpContextAccessor.HttpContext!.InsertParameterPagination(queryable, pagination.RecordsNumber);
            var modelo = await queryable.OrderBy(x => x.Name).Paginate(pagination).ToListAsync();

            return new ActionResponse<IEnumerable<ProductCategory>>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<ProductCategory>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<ProductCategory>> GetAsync(Guid id)
    {
        try
        {
            var modelo = await _context.ProductCategories.FindAsync(id);
            if (modelo == null)
            {
                return new ActionResponse<ProductCategory>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            return new ActionResponse<ProductCategory>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<ProductCategory>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<ProductCategory>> UpdateAsync(ProductCategory modelo)
    {
        await _transactionManager.BeginTransactionAsync();

        try
        {
            _context.ProductCategories.Update(modelo);

            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<ProductCategory>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<ProductCategory>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<ProductCategory>> AddAsync(ProductCategory modelo, string email)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<ProductCategory>
                {
                    WasSuccess = false,
                    Message = "Problemas de Validacion de Usuario"
                };
            }
            modelo.CorporationId = Convert.ToInt32(user.CorporationId);
            _context.ProductCategories.Add(modelo);
            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<ProductCategory>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<ProductCategory>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var DataRemove = await _context.ProductCategories.FindAsync(id);
            if (DataRemove == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            _context.ProductCategories.Remove(DataRemove);

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