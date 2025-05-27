using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Spix.AppFront.Helpers;
using Spix.Core.EntitiesInven;
using Spix.HttpServices;

namespace Spix.AppFront.Pages.EntitiesInven.TransferPage;

public partial class CreateTransfer
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    private Transfer Transfer = new();

    private FormTransfer? FormTransfer { get; set; }

    private string BaseUrl = "/api/v1/transfers";
    private string BaseView = "/transfers";

    private async Task Create()
    {
        var responseHttp = await _repository.PostAsync<Transfer, Transfer>($"{BaseUrl}", Transfer);
        // Centralizamos el manejo de errores
        bool errorHandled = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandled)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        Transfer = responseHttp.Response!;
        FormTransfer!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo($"{BaseView}/details/{Transfer.TransferId}");
    }

    private void Return()
    {
        FormTransfer!.FormPostedSuccessfully = true;
        _navigationManager.NavigateTo($"{BaseView}");
    }
}