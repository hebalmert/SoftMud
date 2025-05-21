using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.Win32;
using Spix.AppFront.Helpers;
using Spix.Core.EntitiesGen;
using Spix.HttpServices;

namespace Spix.AppFront.Pages.EntitiesGen.RegisterPage;

public partial class EditRegister
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    private Register? Register;
    private string BaseUrl = "/api/v1/registers";
    private string BaseView = "/registers";

    [Parameter] public Guid Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await _repository.GetAsync<Register>($"{BaseUrl}/{Id}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        Register = responseHttp.Response;
    }

    private async Task Edit()
    {
        var responseHttp = await _repository.PutAsync($"{BaseUrl}", Register);
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
}