using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spix.AppFront.Helpers;
using Spix.Core.EntitiesContratos;
using Spix.HttpServices;

namespace Spix.AppFront.Pages.EntitiesContratos.ContractClientPage;

public partial class EditContractClient
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;
    [Inject] private IDialogService _dialogService { get; set; } = null!;

    [Inject] private ISnackbar _snackbar { get; set; } = null!;

    private ContractClient? ContractClient;
    private string BaseUrl = "/api/v1/contractclients";
    private string BaseView = "/contractclients";

    [Parameter] public Guid Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await _repository.GetAsync<ContractClient>($"{BaseUrl}/{Id}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        ContractClient = responseHttp.Response;
    }

    private async Task Edit()
    {
        var responseHttp = await _repository.PutAsync($"{BaseUrl}", ContractClient);
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        _navigationManager.NavigateTo($"{BaseView}");
    }

    private void Return()
    {
        _navigationManager.NavigateTo($"{BaseView}");
    }

    private async Task ProcesarAction()
    {
        _snackbar.Configuration.SnackbarVariant = Variant.Filled;
        _snackbar.Add("¿Al Cambiar Estatus de Creando a Procesando no podra devolver el Status, Continuar?", Severity.Success, config =>
        {
            config.Action = "Sí";
            config.OnClick = async snackbar =>
            {
                var responseHttp = await _repository.GetAsync($"/api/v1/contractclients/procesando/{ContractClient!.ContractClientId}");
                var errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
                if (errorHandler)
                {
                    _navigationManager.NavigateTo($"{BaseView}");
                    return;
                }
                _navigationManager.NavigateTo($"{BaseView}");
                return;
            };
        });
    }
}