using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spix.CoreShared.ResponsesSec;
using Spix.UnitOfWork.InterfacesSecure;

namespace Spix.AppBack.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/accounts")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountUnitOfWork _accountUnitOfWork;
    private readonly IConfiguration _configuration;

    public AccountsController(IAccountUnitOfWork accountUnitOfWork, IConfiguration configuration)
    {
        _accountUnitOfWork = accountUnitOfWork;
        _configuration = configuration;
    }

    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginDTO modelo)
    {
        var response = await _accountUnitOfWork.LoginAsync(modelo);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpPost("RecoverPassword")]
    public async Task<IActionResult> RecoverPasswordAsync([FromBody] EmailDTO modelo)
    {
        var response = await _accountUnitOfWork.RecoverPasswordAsync(modelo, _configuration["UrlFrontend"]!);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordDTO modelo)
    {
        var response = await _accountUnitOfWork.ResetPasswordAsync(modelo);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpPost("changePassword")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO modelo)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        if (string.IsNullOrWhiteSpace(User.Identity!.Name!))
        {
            return Unauthorized("El usuario no está autenticado.");
        }
        string UserName = User.Identity!.Name!;

        var response = await _accountUnitOfWork.ChangePasswordAsync(modelo, UserName);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }

    [HttpGet("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmailAsync(string userId, string token)
    {
        token = token.Replace(" ", "+");
        var response = await _accountUnitOfWork.ConfirmEmailAsync(userId, token);
        if (!response.WasSuccess)
        {
            return BadRequest(response.Message);
        }
        return Ok(response.Result);
    }
}