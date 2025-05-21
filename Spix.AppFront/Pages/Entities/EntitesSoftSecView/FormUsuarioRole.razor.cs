using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Spix.AppFront.Helpers;
using Spix.Core.EntitesSoftSec;
using Spix.CoreShared.Enum;
using Spix.HttpServices;

namespace Spix.AppFront.Pages.Entities.EntitesSoftSecView;

public partial class FormUsuarioRole
{
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    private EnumItemModel? SelectedUserType;
    private List<EnumItemModel>? ListUserType;

    [Parameter, EditorRequired] public UsuarioRole UsuarioRole { get; set; } = null!;
    [Parameter, EditorRequired] public EventCallback OnSubmit { get; set; }
    [Parameter, EditorRequired] public EventCallback ReturnAction { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadRoles();
    }

    private async Task LoadRoles()
    {
        var responseHTTP = await _repository.GetAsync<List<EnumItemModel>>($"api/v1/usuarioRoles/loadCombo");
        bool errorHandled = await _responseHandler.HandleErrorAsync(responseHTTP);
        if (errorHandled)
        {
            _navigationManager.NavigateTo("/usuarios");
            return;
        }
        ListUserType = responseHTTP.Response;
    }

    private void UsertTypeChanged(EnumItemModel modelo)
    {
        if (modelo.Name == "Usuario") { UsuarioRole.UserType = UserType.Usuario; }
        if (modelo.Name == "Auxiliar") { UsuarioRole.UserType = UserType.Auxiliar; }
        if (modelo.Name == "Cajero") { UsuarioRole.UserType = UserType.Cajero; }
        if (modelo.Name == "Tecnico") { UsuarioRole.UserType = UserType.Tecnico; }
        if (modelo.Name == "Cobrador") { UsuarioRole.UserType = UserType.Cobrador; }
        SelectedUserType = modelo;
    }
}