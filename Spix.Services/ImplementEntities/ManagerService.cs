using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Spix.Core.Entities;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.CoreShared.ResponsesSec;
using Spix.Helper;
using Spix.Helper.Extensions;
using Spix.Helper.Helpers;
using Spix.Helper.Transactions;
using Spix.Infrastructure;
using Spix.Services.InterfacesEntities;

namespace Spix.Services.ImplementEntities;

public class ManagerService : IManagerService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITransactionManager _transactionManager;
    private readonly HttpErrorHandler _httpErrorHandler;
    private readonly IFileStorage _fileStorage;
    private readonly IUserHelper _userHelper;
    private readonly IEmailHelper _emailHelper;
    private readonly ImgSetting _imgOption;

    public ManagerService(DataContext context, IHttpContextAccessor httpContextAccessor,
        ITransactionManager transactionManager, IMemoryCache cache, IFileStorage fileStorage,
        IUserHelper userHelper, IEmailHelper emailHelper, IOptions<ImgSetting> ImgOption)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _transactionManager = transactionManager;
        _fileStorage = fileStorage;
        _userHelper = userHelper;
        _emailHelper = emailHelper;
        _imgOption = ImgOption.Value;
        _httpErrorHandler = new HttpErrorHandler();
    }

    public async Task<ActionResponse<IEnumerable<Manager>>> GetAsync(PaginationDTO pagination)
    {
        try
        {
            var queryable = _context.Managers.Include(x => x.Corporation).AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                queryable = queryable.Where(x => x.FullName!.ToLower().Contains(pagination.Filter.ToLower()));
            }

            await _httpContextAccessor.HttpContext!.InsertParameterPagination(queryable, pagination.RecordsNumber);
            var modelo = await queryable.OrderBy(x => x.FullName).Paginate(pagination).ToListAsync();

            return new ActionResponse<IEnumerable<Manager>>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<IEnumerable<Manager>>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Manager>> GetAsync(int id)
    {
        try
        {
            var modelo = await _context.Managers.Include(x => x.Corporation).FirstOrDefaultAsync(x => x.ManagerId == id);
            if (modelo == null)
            {
                return new ActionResponse<Manager>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }

            return new ActionResponse<Manager>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            return await _httpErrorHandler.HandleErrorAsync<Manager>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Manager>> UpdateAsync(Manager modelo, string frontUrl)
    {
        await _transactionManager.BeginTransactionAsync();

        try
        {
            Manager NewModelo = new()
            {
                ManagerId = modelo.ManagerId,
                FirstName = modelo.FirstName,
                LastName = modelo.LastName,
                FullName = $"{modelo.FirstName} {modelo.LastName}",
                Nro_Document = modelo.Nro_Document,
                PhoneNumber = modelo.PhoneNumber,
                Address = modelo.Address,
                UserName = modelo.UserName,
                CorporationId = modelo.CorporationId,
                Job = modelo.Job,
                UserType = modelo.UserType,
                Imagen = modelo.Imagen,
                Active = modelo.Active,
            };
            if (modelo.ImgBase64 != null)
            {
                NewModelo.ImgBase64 = modelo.ImgBase64;
            }

            if (!string.IsNullOrEmpty(modelo.ImgBase64))
            {
                string guid;
                if (modelo.Imagen == null)
                {
                    guid = Guid.NewGuid().ToString() + ".jpg";
                }
                else
                {
                    guid = modelo.Imagen;
                }
                var imageId = Convert.FromBase64String(modelo.ImgBase64);
                NewModelo.Imagen = await _fileStorage.UploadImage(imageId, _imgOption.ImgManager!, guid);
            }
            _context.Managers.Update(NewModelo);
            await _transactionManager.SaveChangesAsync();

            User UserCurrent = await _userHelper.GetUserAsync(modelo.UserName);
            if (UserCurrent != null)
            {
                UserCurrent.FirstName = modelo.FirstName;
                UserCurrent.LastName = modelo.LastName;
                UserCurrent.FullName = $"{modelo.FirstName} {modelo.LastName}";
                UserCurrent.PhoneNumber = modelo.PhoneNumber;
                UserCurrent.PhotoUser = modelo.Imagen;
                UserCurrent.JobPosition = modelo.Job;
                UserCurrent.Active = modelo.Active;
                IdentityResult result = await _userHelper.UpdateUserAsync(UserCurrent);
            }
            else
            {
                if (modelo.Active)
                {
                    Response response = await AcivateUser(modelo, frontUrl);
                    if (response.IsSuccess == false)
                    {
                        var guid = modelo.Imagen;
                        _fileStorage.DeleteImage(_imgOption.ImgManager!, guid!);
                        await _transactionManager.RollbackTransactionAsync();
                        return new ActionResponse<Manager>
                        {
                            WasSuccess = true,
                            Message = "No se ha podido crear el Usuario, Intentelo de nuevo"
                        };
                    }
                }
            }

            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<Manager>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Manager>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<Manager>> AddAsync(Manager modelo, string frontUrl)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            User CheckEmail = await _userHelper.GetUserAsync(modelo.UserName);
            if (CheckEmail != null)
            {
                return new ActionResponse<Manager>
                {
                    WasSuccess = true,
                    Message = "El Correo ingresado ya se encuentra reservado, debe cambiarlo."
                };
            }

            modelo.FullName = $"{modelo.FirstName} {modelo.LastName}";
            modelo.UserType = UserType.Usuario;
            if (!string.IsNullOrEmpty(modelo.ImgBase64))
            {
                string guid = Guid.NewGuid().ToString() + ".jpg";
                var imageId = Convert.FromBase64String(modelo.ImgBase64);
                modelo.Imagen = await _fileStorage.UploadImage(imageId, _imgOption.ImgManager!, guid);
            }

            _context.Managers.Add(modelo);
            await _transactionManager.SaveChangesAsync();

            //Registro del Usuario en User
            if (modelo.Active)
            {
                Response response = await AcivateUser(modelo, frontUrl);
                if (!response.IsSuccess)
                {
                    var guid = modelo.Imagen;
                    _fileStorage.DeleteImage(_imgOption.ImgManager!, guid!);
                    await _transactionManager.RollbackTransactionAsync();
                    return new ActionResponse<Manager>
                    {
                        WasSuccess = true,
                        Message = "No se ha podido crear el Usuario, Intentelo de nuevo"
                    };
                }
            }

            await _transactionManager.CommitTransactionAsync();

            return new ActionResponse<Manager>
            {
                WasSuccess = true,
                Result = modelo
            };
        }
        catch (Exception ex)
        {
            await _transactionManager.RollbackTransactionAsync();
            return await _httpErrorHandler.HandleErrorAsync<Manager>(ex); // ✅ Manejo de errores automático
        }
    }

    public async Task<ActionResponse<bool>> DeleteAsync(int id)
    {
        await _transactionManager.BeginTransactionAsync();
        try
        {
            var DataRemove = await _context.Managers.FindAsync(id);
            if (DataRemove == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "Problemas para Enconstrar el Registro Indicado"
                };
            }
            var user = await _userHelper.GetUserAsync(DataRemove.UserName);
            var RemoveRoleDetail = await _context.UserRoleDetails.Where(x => x.UserId == user.Id).ToListAsync();
            if (RemoveRoleDetail != null)
            {
                _context.UserRoleDetails.RemoveRange(RemoveRoleDetail!);
            }
            await _userHelper.DeleteUser(DataRemove.UserName);

            _context.Managers.Remove(DataRemove);

            if (DataRemove.Imagen is not null)
            {
                var response = _fileStorage.DeleteImage(_imgOption.ImgManager!, DataRemove.Imagen);
                if (!response)
                {
                    return new ActionResponse<bool>
                    {
                        WasSuccess = false,
                        Message = "Se Elimino el Registro pero Sin la Imagen"
                    };
                }
            }

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

    private async Task<Response> AcivateUser(Manager manager, string frontUrl)
    {
        User user = await _userHelper.AddUserUsuarioAsync(manager.FirstName, manager.LastName, manager.UserName,
            manager.PhoneNumber, manager.Address, manager.Job, manager.CorporationId, manager.Imagen!, "Manager", manager.Active, manager.UserType);

        //Envio de Correo con Token de seguridad para Verificar el correo
        string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

        // Construir la URL sin `Url.Action`
        string tokenLink = $"{frontUrl}/api/accounts/ConfirmEmail?userid={user.Id}&token={myToken}";

        string subject = "Activacion de Cuenta";
        string body = ($"De: NexxtPlanet" +
            $"<h1>Email Confirmation</h1>" +
            $"<p>" +
            $"Su Clave Temporal es: <h2> \"{user.Pass}\"</h2>" +
            $"</p>" +
            $"Para Activar su vuenta, " +
            $"Has Click en el siguiente Link:</br></br><strong><a href = \"{tokenLink}\">Confirmar Correo</a></strong>");

        Response response = await _emailHelper.ConfirmarCuenta(user.UserName!, user.FullName!, subject, body);
        if (response.IsSuccess == false)
        {
            return response;
        }
        return response;
    }
}