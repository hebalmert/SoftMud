using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Spix.AppFront.Helpers;
using Spix.Core.EntitiesInven;
using Spix.HttpServices;

namespace Spix.AppFront.Pages.EntitiesInven.SerialPage;

public partial class EditSerialsDetails
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    private CargueDetail? CargueDetail;

    private string BaseUrl = "/api/v1/cargueDetails";
    private string BaseView = "/Serials";

    [Parameter] public Guid Id { get; set; }  //Es CargueDetailId

    protected override async Task OnInitializedAsync()
    {
        var responseHttp = await _repository.GetAsync<CargueDetail>($"{BaseUrl}/{Id}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        CargueDetail = responseHttp.Response;
    }

    private async Task Edit()
    {
        CargueDetail NewModel = new()
        {
            CargueDetailId = CargueDetail!.CargueDetailId,
            CargueId = CargueDetail.CargueId,
            MacWlan = CargueDetail.MacWlan,
            DateCargue = CargueDetail.DateCargue,
            Comment = CargueDetail.Comment,
            Status = CargueDetail.Status,
            CorporationId = CargueDetail.CorporationId
        };
        var responseHttp = await _repository.PutAsync($"{BaseUrl}", NewModel);
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