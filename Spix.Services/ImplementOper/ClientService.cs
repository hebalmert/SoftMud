using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Spix.Core.Entities;
using Spix.Core.EntitiesOper;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Pagination;
using Spix.CoreShared.Responses;
using Spix.CoreShared.ResponsesSec;
using Spix.Helper;
using Spix.Helper.Extensions;
using Spix.Helper.Helpers;
using Spix.Helper.Mappings;
using Spix.Helper.Transactions;
using Spix.Infrastructure;
using Spix.Services.InterfacesOper;

namespace Spix.Services.ImplementOper
{
    public class ClientService : IClientService
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapperService _mapperService;
        private readonly ITransactionManager _transactionManager;
        private readonly HttpErrorHandler _httpErrorHandler;
        private readonly IFileStorage _fileStorage;
        private readonly IUserHelper _userHelper;
        private readonly IEmailHelper _emailHelper;
        private readonly ImgSetting _imgOption;

        public ClientService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapperService mapperService,
            ITransactionManager transactionManager, IMemoryCache cache, IFileStorage fileStorage,
            IUserHelper userHelper, IEmailHelper emailHelper, IOptions<ImgSetting> ImgOption)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapperService = mapperService;
            _transactionManager = transactionManager;
            _fileStorage = fileStorage;
            _userHelper = userHelper;
            _emailHelper = emailHelper;
            _imgOption = ImgOption.Value;
            _httpErrorHandler = new HttpErrorHandler();
        }

        public async Task<ActionResponse<IEnumerable<Client>>> GetAsync(PaginationDTO pagination, string email)
        {
            try
            {
                var user = await _userHelper.GetUserAsync(email);
                if (user == null)
                {
                    return new ActionResponse<IEnumerable<Client>>
                    {
                        WasSuccess = false,
                        Message = "Problemas de Validacion de Usuario"
                    };
                }

                var queryable = _context.Clients.Where(x => x.CorporationId == user.CorporationId).AsQueryable();

                if (!string.IsNullOrWhiteSpace(pagination.Filter))
                {
                    queryable = queryable.Where(x => x.FullName!.ToLower().Contains(pagination.Filter.ToLower()));
                }

                await _httpContextAccessor.HttpContext!.InsertParameterPagination(queryable, pagination.RecordsNumber);
                var modelo = await queryable.OrderBy(x => x.FullName).Paginate(pagination).ToListAsync();

                return new ActionResponse<IEnumerable<Client>>
                {
                    WasSuccess = true,
                    Result = modelo
                };
            }
            catch (Exception ex)
            {
                return await _httpErrorHandler.HandleErrorAsync<IEnumerable<Client>>(ex); // ✅ Manejo de errores automático
            }
        }

        public async Task<ActionResponse<Client>> GetAsync(Guid id)
        {
            try
            {
                var modelo = await _context.Clients.Include(x => x.DocumentType).FirstOrDefaultAsync(x => x.ClientId == id);
                if (modelo == null)
                {
                    return new ActionResponse<Client>
                    {
                        WasSuccess = false,
                        Message = "Problemas para Enconstrar el Registro Indicado"
                    };
                }

                return new ActionResponse<Client>
                {
                    WasSuccess = true,
                    Result = modelo
                };
            }
            catch (Exception ex)
            {
                return await _httpErrorHandler.HandleErrorAsync<Client>(ex); // ✅ Manejo de errores automático
            }
        }

        public async Task<ActionResponse<Client>> UpdateAsync(Client modelo, string frontUrl)
        {
            await _transactionManager.BeginTransactionAsync();

            try
            {
                Client NewModelo = _mapperService.Map<Client, Client>(modelo);
                NewModelo.FullName = $"{NewModelo.FirstName} {NewModelo.LastName}";
                if (modelo.ImgBase64 != null)
                {
                    NewModelo.ImgBase64 = modelo.ImgBase64;
                }

                if (!string.IsNullOrEmpty(modelo.ImgBase64))
                {
                    string guid;
                    if (modelo.Photo == null)
                    {
                        guid = Guid.NewGuid().ToString() + ".jpg";
                    }
                    else
                    {
                        guid = modelo.Photo;
                    }
                    var imageId = Convert.FromBase64String(modelo.ImgBase64);
                    NewModelo.Photo = await _fileStorage.UploadImage(imageId, _imgOption.ImgManager!, guid);
                }
                _context.Clients.Update(NewModelo);
                await _transactionManager.SaveChangesAsync();

                User UserCurrent = await _userHelper.GetUserAsync(modelo.UserName);
                if (UserCurrent != null)
                {
                    UserCurrent.FirstName = modelo.FirstName;
                    UserCurrent.LastName = modelo.LastName;
                    UserCurrent.FullName = $"{modelo.FirstName} {modelo.LastName}";
                    UserCurrent.PhoneNumber = modelo.PhoneNumber;
                    UserCurrent.PhotoUser = modelo.Photo;
                    UserCurrent.JobPosition = "Client";
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
                            var guid = modelo.Photo;
                            _fileStorage.DeleteImage(_imgOption.ImgManager!, guid!);
                            await _transactionManager.RollbackTransactionAsync();
                            return new ActionResponse<Client>
                            {
                                WasSuccess = true,
                                Message = "No se ha podido crear el Usuario, Intentelo de nuevo"
                            };
                        }
                    }
                }

                await _transactionManager.CommitTransactionAsync();

                return new ActionResponse<Client>
                {
                    WasSuccess = true,
                    Result = modelo
                };
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackTransactionAsync();
                return await _httpErrorHandler.HandleErrorAsync<Client>(ex); // ✅ Manejo de errores automático
            }
        }

        public async Task<ActionResponse<Client>> AddAsync(Client modelo, string email, string frontUrl)
        {
            await _transactionManager.BeginTransactionAsync();
            try
            {
                User CheckEmail = await _userHelper.GetUserAsync(modelo.UserName);
                if (CheckEmail != null)
                {
                    return new ActionResponse<Client>
                    {
                        WasSuccess = true,
                        Message = "El Correo ingresado ya se encuentra reservado, debe cambiarlo."
                    };
                }

                var user = await _userHelper.GetUserAsync(email);
                if (user == null)
                {
                    return new ActionResponse<Client>
                    {
                        WasSuccess = false,
                        Message = "Problemas de Validacion de Usuario"
                    };
                }

                modelo.DateCreated = DateTime.Now;
                modelo.FullName = $"{modelo.FirstName} {modelo.LastName}";
                modelo.UserType = UserType.Cliente;
                modelo.CorporationId = Convert.ToInt32(user.CorporationId);
                if (!string.IsNullOrEmpty(modelo.ImgBase64))
                {
                    string guid = Guid.NewGuid().ToString() + ".jpg";
                    var imageId = Convert.FromBase64String(modelo.ImgBase64);
                    modelo.Photo = await _fileStorage.UploadImage(imageId, _imgOption.ImgClient!, guid);
                }

                _context.Clients.Add(modelo);
                await _transactionManager.SaveChangesAsync();

                //Registro del Usuario en User
                if (modelo.Active)
                {
                    Response response = await AcivateUser(modelo, frontUrl);
                    if (!response.IsSuccess)
                    {
                        var guid = modelo.Photo;
                        _fileStorage.DeleteImage(_imgOption.ImgManager!, guid!);
                        await _transactionManager.RollbackTransactionAsync();
                        return new ActionResponse<Client>
                        {
                            WasSuccess = true,
                            Message = "No se ha podido crear el Usuario, Intentelo de nuevo"
                        };
                    }
                }

                await _transactionManager.CommitTransactionAsync();

                return new ActionResponse<Client>
                {
                    WasSuccess = true,
                    Result = modelo
                };
            }
            catch (Exception ex)
            {
                await _transactionManager.RollbackTransactionAsync();
                return await _httpErrorHandler.HandleErrorAsync<Client>(ex); // ✅ Manejo de errores automático
            }
        }

        public async Task<ActionResponse<bool>> DeleteAsync(Guid id)
        {
            await _transactionManager.BeginTransactionAsync();
            try
            {
                var DataRemove = await _context.Clients.FindAsync(id);
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

                _context.Clients.Remove(DataRemove);

                if (DataRemove.Photo is not null)
                {
                    var response = _fileStorage.DeleteImage(_imgOption.ImgClient!, DataRemove.Photo);
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

        private async Task<Response> AcivateUser(Client modelo, string frontUrl)
        {
            User user = await _userHelper.AddUserUsuarioAsync(modelo.FirstName, modelo.LastName, modelo.UserName,
                modelo.PhoneNumber, modelo.Address, "Client", modelo.CorporationId, modelo.Photo!, "Client", modelo.Active, modelo.UserType);

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
}