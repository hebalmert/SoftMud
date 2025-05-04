using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Spix.AppFront.Helpers;
using Spix.Core.EntitiesNet;
using Spix.HttpServices;

namespace Spix.AppFront.Pages.EntitiesNet.IpNetPage;

public partial class CreateIpNet
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    private IpNet IpNet = new();

    private string BaseUrl = "/api/v1/ipnets";
    private string BaseView = "/ipnets";

    protected override void OnInitialized()
    {
        IpNet.Active = true;
    }
    private async Task Create()
    {
        var responseHttp = await _repository.PostAsync($"{BaseUrl}", IpNet);
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