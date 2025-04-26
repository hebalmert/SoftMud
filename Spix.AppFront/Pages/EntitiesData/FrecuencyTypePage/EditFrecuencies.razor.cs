using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Spix.AppFront.Helpers;
using Spix.Core.EntitesSoftSec;
using Spix.Core.EntitiesData;
using Spix.HttpServices;

namespace Spix.AppFront.Pages.EntitiesData.FrecuencyTypePage;

public partial class EditFrecuencies
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    private Frecuency? Frecuency;

    private string BaseUrl = "/api/v1/frecuencies";
    private string BaseView = "/frecuencytypes/details";

    [Parameter] public int Id { get; set; }  //FrecuencyId

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await _repository.GetAsync<Frecuency>($"{BaseUrl}/{Id}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        Frecuency = responseHttp.Response;
    }

    private async Task Edit()
    {
        var responseHttp = await _repository.PutAsync($"{BaseUrl}", Frecuency);
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}/{Id}");
            return;
        }
        _navigationManager.NavigateTo($"{BaseView}/{Frecuency!.FrecuencyTypeId}");
    }

    private void Return()
    {
        _navigationManager.NavigateTo($"{BaseView}/{Frecuency!.FrecuencyTypeId}");
    }
}