using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Spix.AppFront.Helpers;
using Spix.Core.Entities;
using Spix.HttpServices;

namespace Spix.AppFront.Pages.Entities.SoftPlanPage;

public partial class CreateSoftPlan
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    private SoftPlan _softplan = new();

    private string BaseUrl = "/api/v1/softplans";
    private string BaseView = "/softplans";

    private async Task Create()
    {
        var responseHttp = await _repository.PostAsync($"{BaseUrl}", _softplan);
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