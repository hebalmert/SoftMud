using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Spix.AppFront.Helpers;
using Spix.Core.Entities;
using Spix.Core.EntitiesGen;
using Spix.Core.EntitiesNet;
using Spix.HttpServices;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Spix.AppFront.Pages.EntitiesNet.ServerPage;

public partial class FormServer
{
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    [Parameter, EditorRequired] public Server Server { get; set; } = null!;
    [Parameter, EditorRequired] public EventCallback OnSubmit { get; set; }
    [Parameter, EditorRequired] public EventCallback ReturnAction { get; set; }
    [Parameter, EditorRequired] public bool IsEditControl { get; set; }

    private State? SelectedSate;
    private List<State>? States;
    private City? SelectedCity = new();
    private List<City>? Cities = new();
    private Zone? SelectedZone = new();
    private List<Zone>? Zones = new();
    private Mark? SelectedMark = new();
    private List<Mark>? Marks = new();
    private MarkModel? SelectedMarkModel = new();
    private List<MarkModel>? MarkModels = new();
    private IpNetwork? SelectedIpnetwork = new();
    private List<IpNetwork>? IpNetworks = new();
    private string BaseView = "/corporations";
    private string BaseComboState = "/api/v1/states/loadCombo";
    private string BaseComboCity = "/api/v1/cities/loadCombo";
    private string BaseComboZone = "/api/v1/zones/loadCombo";
    private string BaseComboMark = "/api/v1/marks/loadCombo";
    private string BaseComboMarkModel = "/api/v1/marksmodels/loadCombo";
    private string BaseComboIpNetwork = "/api/v1/ipnetworks/loadCombo";

    protected override async Task OnInitializedAsync()
    {
        await LoadState();
        await LoadMarks();
        await LoadIpNetwork();
    }

    private async Task LoadIpNetwork()
    {
        if (IsEditControl)
        {
            var responseHttp2 = await _repository.GetAsync<List<IpNetwork>>($"{BaseComboIpNetwork}/{Server.IpNetworkId}");
            bool errorHandler2 = await _responseHandler.HandleErrorAsync(responseHttp2);
            if (errorHandler2)
            {
                _navigationManager.NavigateTo($"{BaseView}");
                return;
            }
            IpNetworks = responseHttp2.Response;
            SelectedIpnetwork = IpNetworks!.Where(x => x.IpNetworkId == Server.IpNetworkId)
                    .Select(x => new IpNetwork { IpNetworkId = x.IpNetworkId, Ip = x.Ip })
                    .FirstOrDefault();
            return;
        }

        var responseHttp = await _repository.GetAsync<List<IpNetwork>>($"{BaseComboIpNetwork}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        IpNetworks = responseHttp.Response;
    }

    private void IpNetworkChanged(IpNetwork modelo)
    {
        Server.IpNetworkId = modelo.IpNetworkId;
        SelectedIpnetwork = modelo;
    }

    private async Task LoadMarks()
    {
        var responseHttp = await _repository.GetAsync<List<Mark>>($"{BaseComboMark}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        Marks = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedMark = Marks!.Where(x => x.MarkId == Server.MarkId)
                .Select(x => new Mark { MarkId = x.MarkId, MarkName = x.MarkName })
                .FirstOrDefault();

            await LoadMarkModel(SelectedMark!.MarkId);
        }
    }

    private async Task MarkChanged(Mark modelo)
    {
        Server.MarkId = modelo.MarkId;
        SelectedMark = modelo;
        SelectedMarkModel = new();
        await LoadMarkModel(Server.MarkId);
    }

    private async Task LoadMarkModel(Guid id)
    {
        var responseHttp = await _repository.GetAsync<List<MarkModel>>($"{BaseComboMarkModel}/{id}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        MarkModels = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedMarkModel = MarkModels!.Where(x => x.MarkModelId == Server.MarkModelId)
                .Select(x => new MarkModel { MarkModelId = x.MarkModelId, MarkModelName = x.MarkModelName })
                .FirstOrDefault();
        }
    }

    private void MarkModelChanged(MarkModel modelo)
    {
        Server.MarkModelId = modelo.MarkModelId;
        SelectedMarkModel = modelo;
    }

    private async Task LoadState()
    {
        var responseHttp = await _repository.GetAsync<List<State>>($"{BaseComboState}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        States = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedSate = States!.Where(x => x.StateId == Server.StateId)
                .Select(x => new State { StateId = x.StateId, Name = x.Name })
                .FirstOrDefault();

            await LoadCity(SelectedSate!.StateId);
        }
    }

    private async Task StateChanged(State modelo)
    {
        Server.StateId = modelo.StateId;
        SelectedSate = modelo;
        SelectedCity = new();
        Cities = new();
        SelectedZone = new();
        Zones = new();
        await LoadCity(modelo.StateId);
    }

    private async Task LoadCity(int id)
    {
        var responseHttp = await _repository.GetAsync<List<City>>($"{BaseComboCity}/{id}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        Cities = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedCity = Cities!.Where(x => x.CityId == Server.CityId)
                .Select(x => new City { CityId = x.CityId, Name = x.Name })
                .FirstOrDefault();
            await LoadZone(SelectedCity!.CityId);
        }
    }

    private async Task CityChanged(City modelo)
    {
        Server.CityId = modelo.CityId;
        SelectedCity = modelo;
        SelectedZone = new();
        await LoadZone(Server.CityId);
    }

    private async Task LoadZone(int id)
    {
        var responseHttp = await _repository.GetAsync<List<Zone>>($"{BaseComboZone}/{id}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        Zones = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedZone = Zones!.Where(x => x.ZoneId == Server.ZoneId)
                .Select(x => new Zone { ZoneId = x.ZoneId, ZoneName = x.ZoneName })
                .FirstOrDefault();
        }
    }

    private void ZoneChanged(Zone modelo)
    {
        Server.ZoneId = modelo.ZoneId;
        SelectedZone = modelo;
    }

    private string GetDisplayName<T>(Expression<Func<T>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            var property = memberExpression.Member as PropertyInfo;
            if (property != null)
            {
                var displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute != null)
                {
                    return displayAttribute.Name!;
                }
            }
        }
        return "Texto no definido";
    }
}