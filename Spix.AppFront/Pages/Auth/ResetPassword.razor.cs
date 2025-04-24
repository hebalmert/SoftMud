using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spix.CoreShared.ResponsesSec;
using Spix.HttpServices;

namespace Spix.AppFront.Pages.Auth;

public partial class ResetPassword
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IDialogService DialogService { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IRepository Repository { get; set; } = null!;

    private ResetPasswordDTO resetPasswordDTO = new();

    [CascadingParameter] private IMudDialogInstance _mudDialog { get; set; } = null!;
    [Parameter, SupplyParameterFromQuery] public string Token { get; set; } = string.Empty;

    private async Task ChangePasswordAsync()
    {
        resetPasswordDTO.Token = Token;
        var responseHttp = await Repository.PostAsync("/api/v1/accounts/ResetPassword", resetPasswordDTO);
        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(message!, Severity.Error);
            return;
        }

        Snackbar.Add("Se ha Cambiado con Exito", Severity.Success);
        NavigationManager.NavigateTo("/");
        var closeOnEscapeKey = new DialogOptions() { CloseOnEscapeKey = true };
        await DialogService.ShowAsync<Login>("Iniciar Sesion", closeOnEscapeKey);
    }
}