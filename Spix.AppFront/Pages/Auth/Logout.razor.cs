using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spix.AppFront.AuthenticationProviders;

namespace Spix.AppFront.Pages.Auth;

public partial class Logout
{
    [Inject] private NavigationManager _navigation { get; set; } = null!;
    [Inject] private ILoginService _loginService { get; set; } = null!;
    [CascadingParameter] private IMudDialogInstance _mudDialog { get; set; } = null!;

    private async Task LogoutActionAsync()
    {
        await _loginService.LogoutAsync();
        _navigation.NavigateTo("/");
        CancelAction();
    }

    private void CancelAction()
    {
        _mudDialog.Cancel();
    }
}