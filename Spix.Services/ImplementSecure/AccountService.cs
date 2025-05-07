using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Spix.Core.Entities;
using Spix.CoreShared.Enum;
using Spix.CoreShared.Responses;
using Spix.CoreShared.ResponsesSec;
using Spix.Helper;
using Spix.Infrastructure;
using Spix.Services.InterfacesSecure;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Spix.Services.ImplementSecure;

public class AccountService : IAccountService
{
    private readonly DataContext _context;
    private readonly IUserHelper _userHelper;
    private readonly IEmailHelper _emailHelper;
    private readonly JwtKeySetting _jwtOption;
    private readonly ImgSetting _imgOption;

    public AccountService(DataContext context, IUserHelper userHelper,
        IEmailHelper emailHelper, IOptions<ImgSetting> ImgOption,
        IOptions<JwtKeySetting> jwtOption)
    {
        _context = context;
        _userHelper = userHelper;
        _emailHelper = emailHelper;
        _jwtOption = jwtOption.Value;
        _imgOption = ImgOption.Value;
    }

    public async Task<ActionResponse<TokenDTO>> LoginAsync(LoginDTO modelo)
    {
        string imgUsuario = string.Empty;
        string ImagenDefault = _imgOption.ImgNoImage;
        string BaseUrl = _imgOption.ImgBaseUrl;

        var result = await _userHelper.LoginAsync(modelo);
        if (result.Succeeded)
        {
            //Consulto User de IdentityUser
            var user = await _userHelper.GetUserAsync(modelo.Email);
            if (!user.Active)
            {
                return new ActionResponse<TokenDTO>
                {
                    WasSuccess = false,
                    Message = "El Usuario se Encuentra Inactivo, contacte al Administrador del Sistema"
                };
            }
            var RolesUsuario = _context.UserRoleDetails.Where(c => c.UserId == user.Id).ToList();
            if (RolesUsuario.Count == 0)
            {
                return new ActionResponse<TokenDTO>
                {
                    WasSuccess = false,
                    Message = "Este Usuario esta activo pero no tiene ningun Role Asignado..."
                };
            }
            var RolUsuario = RolesUsuario.FirstOrDefault(c => c.UserType == UserType.Admin);
            if (RolUsuario == null)
            {
                var CheckCorporation = await _context.Corporations.FirstOrDefaultAsync(x => x.CorporationId == user.CorporationId);
                DateTime hoy = DateTime.Today;
                DateTime current = CheckCorporation!.DateEnd;
                if (!CheckCorporation.Active)
                {
                    return new ActionResponse<TokenDTO>
                    {
                        WasSuccess = false,
                        Message = "La Corporacion que trata de Acceder se encuentra Inactiva, Contacte al Administrador del Sistema"
                    };
                }
                if (current <= hoy)
                {
                    return new ActionResponse<TokenDTO>
                    {
                        WasSuccess = false,
                        Message = "El Tiempo del plan se ha cumplido, debe renovar su cuenta"
                    };
                }

                switch (user.UserFrom)
                {
                    case "Client":
                        imgUsuario = user.PhotoUser != null ? $"{BaseUrl}/ImgClient/{user.PhotoUser}" : ImagenDefault;
                        break;

                    case "Manager":
                        imgUsuario = user.PhotoUser != null ? $"{BaseUrl}/ImgManager/{user.PhotoUser}" : ImagenDefault;
                        break;

                    case "UsuarioSoftware":
                        imgUsuario = user.PhotoUser != null ? $"{BaseUrl}/ImgUsuarios/{user.PhotoUser}" : ImagenDefault;
                        break;
                }
            }
            return new ActionResponse<TokenDTO>
            {
                WasSuccess = true,
                Result = BuildToken(user, imgUsuario)
            };
        }

        if (result.IsLockedOut)
        {
            return new ActionResponse<TokenDTO>
            {
                WasSuccess = false,
                Message = "Se Encuentra temporalmente Bloqueado"
            };
        }

        if (result.IsNotAllowed)
        {
            return new ActionResponse<TokenDTO>
            {
                WasSuccess = false,
                Message = "Lo Siento, No Tiene Acceso al Sistema"
            };
        }

        return new ActionResponse<TokenDTO>
        {
            WasSuccess = false,
            Message = "Usuario o Clave Erroneos"
        };
    }

    public async Task<ActionResponse<bool>> RecoverPasswordAsync(EmailDTO modelo, string frontUrl)
    {
        var user = await _userHelper.GetUserAsync(modelo.Email);
        if (user == null)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = "Registro no Encontrado"
            };
        }

        Response response = await SendRecoverEmailAsync(user, frontUrl);
        if (response.IsSuccess)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = true,
                Message = response.Message
            };
        }

        return new ActionResponse<bool>
        {
            WasSuccess = false,
            Message = response.Message
        };
    }

    public async Task<ActionResponse<bool>> ResetPasswordAsync(ResetPasswordDTO modelo)
    {
        var user = await _userHelper.GetUserAsync(modelo.Email);
        if (user == null)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = "Problemas para Validar el Usuario"
            };
        }

        var result = await _userHelper.ResetPasswordAsync(user, modelo.Token, modelo.NewPassword);
        if (result.Succeeded)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = true,
                Message = "Proceso realizado con Exito"
            };
        }
        return new ActionResponse<bool>
        {
            WasSuccess = false,
            Message = result.Errors.FirstOrDefault()!.Description
        };
    }

    public async Task<ActionResponse<bool>> ChangePasswordAsync(ChangePasswordDTO modelo, string UserName)
    {
        var user = await _userHelper.GetUserAsync(UserName);
        if (user == null)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = "Problemas para conseguir el Usuario"
            };
        }

        var result = await _userHelper.ChangePasswordAsync(user, modelo.CurrentPassword, modelo.NewPassword);
        if (!result.Succeeded)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = result.Errors.FirstOrDefault()!.Description
            };
        }

        return new ActionResponse<bool>
        {
            WasSuccess = true,
            Message = "Proceso realizado con Exito"
        };
    }

    public async Task<ActionResponse<bool>> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userHelper.GetUserAsync(new Guid(userId));
        if (user == null)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = "Problemas para conseguir el Usuario"
            };
        }

        var result = await _userHelper.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = result.Errors.FirstOrDefault()!.Description
            };
        }

        return new ActionResponse<bool>
        {
            WasSuccess = true,
            Message = "Proceso realizado con Exito"
        };
    }

    private async Task<Response> SendRecoverEmailAsync(User user, string frontUrl)
    {
        var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

        // Construir la URL sin `Url.Action`
        string tokenLink = $"{frontUrl}/api/accounts/ResetPassword?userid={user.Id}&token={myToken}";

        string subject = "Recuperacion de Clave";
        string body = ($"De: NexxtPlanet" +
            $"<h1>Para Recuperar su Clave</h1>" +
            $"<p>" +
            $"Para Crear una clave nueva " +
            $"Has Click en el siguiente Link:</br></br><strong><a href = \"{tokenLink}\">Cambiar Clave</a></strong>");

        Response response = await _emailHelper.ConfirmarCuenta(user.UserName!, user.FullName!, subject, body);
        if (response.IsSuccess == false)
        {
            return response;
        }
        return response;
    }

    private TokenDTO BuildToken(User user, string imgUsuario)
    {
        string NomCompa;
        string LogoCompa;
        var RolesUsuario = _context.UserRoleDetails.Where(c => c.UserId == user.Id).ToList();
        var RolUsuario = RolesUsuario.Where(c => c.UserType == UserType.Admin).FirstOrDefault();
        if (RolUsuario != null)
        {
            //TODO: Cambio de Path para Imagenes
            NomCompa = "Nexxplanet LLC";
            LogoCompa = _imgOption.LogoSoftware;
            imgUsuario = _imgOption.LogoSoftware;
        }
        else
        {
            var compname = _context.Corporations.FirstOrDefault(x => x.CorporationId == user.CorporationId);
            NomCompa = compname!.Name!;
            LogoCompa = compname!.ImageFullPath;
        }
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email!),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Photo", imgUsuario),
                new Claim("CorpName", NomCompa),
                new Claim("LogoCorp", LogoCompa),
            };
        // Agregar los roles del usuario a los claims
        foreach (var item in RolesUsuario)
        {
            claims.Add(new Claim(ClaimTypes.Role, item.UserType.ToString()!));
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOption.jwtKey!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddDays(30);
        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return new TokenDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration
        };
    }
}