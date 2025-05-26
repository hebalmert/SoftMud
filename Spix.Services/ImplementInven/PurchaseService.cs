﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Spix.Core.EntitiesGen;
using Spix.Core.EntitiesInven;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.ReportsDTO;
using Spix.CoreShared.Responses;
using Spix.Helper;
using Spix.Helper.Extensions;
using Spix.Helper.Helpers;
using Spix.Helper.Mappings;
using Spix.Helper.Transactions;
using Spix.Infrastructure;
using Spix.Services.InterfacesInven;

namespace Spix.Services.ImplementInven;

public class PurchaseService : IPurchaseService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapperService _mapperService;
    private readonly ITransactionManager _transactionManager;
    private readonly HttpErrorHandler _httpErrorHandler;
    private readonly IUserHelper _userHelper;

    public PurchaseService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapperService mapperService,
        ITransactionManager transactionManager, IMemoryCache cache,
        IUserHelper userHelper)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _mapperService = mapperService;
        _transactionManager = transactionManager;
        _userHelper = userHelper;
        _httpErrorHandler = new HttpErrorHandler();
    }

    public async Task<ActionResponse<IEnumerable<EnumItemModel>>> GetComboStatus()
    {
        try
        {
            List<EnumItemModel> list = Enum.GetValues(typeof(PurchaseStatus)).Cast<PurchaseStatus>().Select(c => new EnumItemModel()
            {
                Name = c.ToString(),
                Value = (int)c
            }).ToList();

            return new ActionResponse<IEnumerable<EnumItemModel>>
            {
                WasSuccess = true,
                Result = list
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<EnumItemModel>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<IEnumerable<Purchase>>> GetReporteSellDates(ReportDataDTO pagination, string email)
    {
        try
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<Purchase>>
                {
                    WasSuccess = false,
                    Message = "Problemas de Validacion de Usuario"
                };
            }

            DateTime dateInicio = Convert.ToDateTime(pagination.DateStart);
            DateTime dateFin = Convert.ToDateTime(pagination.DateEnd);

            var queryable = await _context.Purchases.Where(x => x.CorporationId == user.CorporationId && x.Status == PurchaseStatus.Completado
            && x.PurchaseDate >= dateInicio && x.PurchaseDate <= dateFin)
                .Include(x => x.Supplier).Include(x => x.ProductStorage).Include(x => x.PurchaseDetails).ToListAsync();

            return new ActionResponse<IEnumerable<Purchase>>
            {
                WasSuccess = true,
                Result = queryable
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<Purchase>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<IEnumerable<Purchase>>> GetAsync(PaginationDTO pagination, string email)
    {
        try
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<IEnumerable<Purchase>>
                {
                    WasSuccess = false,
                    Message = "Problemas de Validacion de Usuario"
                };
            }

            var queryable = _context.Purchases
                .Include(x => x.ProductStorage)
                .Include(x => x.Supplier)
                .Include(x => x.ProductStorage)
                .Include(x => x.PurchaseDetails)
                .Where(x => x.CorporationId == user.CorporationId).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.Supplier!.Name!.ToLower().Contains(pagination.Filter.ToLower()));
            }

            await _httpContextAccessor.HttpContext!.InsertParameterPagination(queryable, pagination.RecordsNumber);
            var modelo = await queryable.OrderBy(x => x.Supplier!.Name).Paginate(pagination).ToListAsync();

            return new ActionResponse<IEnumerable<Purchase>>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<Purchase>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Purchase>> GetAsync(Guid id)
    {
        try
        {
            var modelo = await _context.Purchases
                .Include(x => x.PurchaseDetails)
                .Include(x => x.Supplier)
                .Include(x => x.ProductStorage)
                .FirstOrDefaultAsync(x => x.PurchaseId == id);

            if (modelo == null)
            {
                return new ActionResponse<Purchase>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            return new ActionResponse<Purchase>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<Purchase>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Purchase>> UpdateAsync(Purchase modelo)
    {
        await _transactionManager.BeginTransactionAsync();

        try
        {
            Purchase NewModelo = _mapperService.Map<Purchase, Purchase>(modelo);
            _context.Purchases.Update(NewModelo);

            await _transactionManager.SaveChangesAsync();

            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<Purchase>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Purchase>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Purchase>> AddAsync(Purchase modelo, string email)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                return new ActionResponse<Purchase>
                {
                    WasSuccess = false,
                    Message = "Problemas de Validacion de Usuario"
                };
            }
            modelo.CorporationId = Convert.ToInt32(user.CorporationId);
            //Para LLevar el control de Consecutivos de Compra
            int ControlCompra = 0;
            var CheckRegister = await _context.Registers.FirstOrDefaultAsync(x => x.CorporationId == modelo.CorporationId);
            if (CheckRegister == null)
            {
                Register nReg = new()
                {
                    RegPurchase = 1,
                    RegSells = 0,
                    CorporationId = modelo.CorporationId
                };
                ControlCompra = 1;
                _context.Registers.Add(nReg);
            }
            else
            {
                CheckRegister.RegPurchase += 1;
                ControlCompra = CheckRegister.RegPurchase;
                _context.Registers.Update(CheckRegister);
            }
            await _context.SaveChangesAsync();
            //Fin...
            modelo.NroPurchase = ControlCompra;

            _context.Purchases.Add(modelo);
            await _transactionManager.SaveChangesAsync();
            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<Purchase>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Purchase>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<bool>> DeleteAsync(Guid id)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var DataRemove = await _context.Purchases.FindAsync(id);
            if (DataRemove == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            _context.Purchases.Remove(DataRemove);

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