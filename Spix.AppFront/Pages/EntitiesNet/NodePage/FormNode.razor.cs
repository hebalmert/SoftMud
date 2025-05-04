using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Spix.AppFront.Helpers;
using Spix.Core.Entities;
using Spix.Core.EntitiesData;
using Spix.Core.EntitiesGen;
using Spix.Core.EntitiesNet;
using Spix.HttpServices;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Spix.AppFront.Pages.EntitiesNet.NodePage;

public partial class FormNode
{
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    [Parameter, EditorRequired] public Node Node { get; set; } = null!;
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
    private Operation? SelectedOperation = new();
    private List<Operation>? Operations = new();
    private Channel? SelectedChannel = new();
    private List<Channel>? Channels = new();
    private Security? SelectedSecurity = new();
    private List<Security>? Securities = new();
    private FrecuencyType? SelectedFrecuencyType = new();
    private List<FrecuencyType>? FrecuencyTypes = new();
    private Frecuency? SelectedFrecuency = new();
    private List<Frecuency>? Frecuencies = new();
    private string BaseView = "/corporations";
    private string BaseComboState = "/api/v1/states/loadCombo";
    private string BaseComboCity = "/api/v1/cities/loadCombo";
    private string BaseComboZone = "/api/v1/zones/loadCombo";
    private string BaseComboMark = "/api/v1/marks/loadCombo";
    private string BaseComboMarkModel = "/api/v1/marksmodels/loadCombo";
    private string BaseComboIpNetwork = "/api/v1/ipnetworks/loadCombo";
    private string BaseComboOperation = "/api/v1/operations/loadCombo";
    private string BaseComboChannels = "/api/v1/channels/loadCombo";
    private string BaseComboSecurity = "/api/v1/securities/loadCombo";
    private string BaseComboFrecuencyType = "/api/v1/frecuencytypes/loadCombo";
    private string BaseComboFrecuency = "/api/v1/frecuencies/loadCombo";

    protected override async Task OnInitializedAsync()
    {
        await LoadState();
        await LoadMarks();
        await LoadIpNetwork();
        await LoadOperation();
        await LoadChannel();
        await LoadSecurity();
        await LoadFrecuencyType();
    }

    private async Task LoadFrecuencyType()
    {
        var responseHttp = await _repository.GetAsync<List<FrecuencyType>>($"{BaseComboFrecuencyType}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        FrecuencyTypes = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedFrecuencyType = FrecuencyTypes!.Where(x => x.FrecuencyTypeId == Node.FrecuencyTypeId)
                    .Select(x => new FrecuencyType { FrecuencyTypeId = x.FrecuencyTypeId, TypeName = x.TypeName })
                    .FirstOrDefault();
            await LoadFrecuency(SelectedFrecuencyType!.FrecuencyTypeId);
        }
    }

    private async Task FrecuencyTypeChanged(FrecuencyType modelo)
    {
        Node.FrecuencyTypeId = modelo.FrecuencyTypeId;
        SelectedFrecuencyType = modelo;
        SelectedFrecuency = new();
        Frecuencies = new();
        await LoadFrecuency(modelo.FrecuencyTypeId);
    }

    private async Task LoadFrecuency(int id)
    {
        var responseHttp = await _repository.GetAsync<List<Frecuency>>($"{BaseComboFrecuency}/{id}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        Frecuencies = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedFrecuency = Frecuencies!.Where(x => x.FrecuencyId == Node.FrecuencyId)
                    .Select(x => new Frecuency { FrecuencyId = x.FrecuencyId, FrecuencyName = x.FrecuencyName })
                    .FirstOrDefault();
        }
    }

    private void FrecuencyChanged(Frecuency modelo)
    {
        Node.FrecuencyId = modelo.FrecuencyId;
        SelectedFrecuency = modelo;
    }

    private async Task LoadSecurity()
    {
        var responseHttp = await _repository.GetAsync<List<Security>>($"{BaseComboSecurity}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        Securities = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedSecurity = Securities!.Where(x => x.SecurityId == Node.SecurityId)
                    .Select(x => new Security { SecurityId = x.SecurityId, SecurityName = x.SecurityName })
                    .FirstOrDefault();
        }
    }

    private void SecurityChanged(Security modelo)
    {
        Node.SecurityId = modelo.SecurityId;
        SelectedSecurity = modelo;
    }

    private async Task LoadChannel()
    {
        var responseHttp = await _repository.GetAsync<List<Channel>>($"{BaseComboChannels}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        Channels = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedChannel = Channels!.Where(x => x.ChannelId == Node.ChannelId)
                    .Select(x => new Channel { ChannelId = x.ChannelId, ChannelName = x.ChannelName })
                    .FirstOrDefault();
        }
    }

    private void ChannelChanged(Channel modelo)
    {
        Node.ChannelId = modelo.ChannelId;
        SelectedChannel = modelo;
    }

    private async Task LoadOperation()
    {
        var responseHttp = await _repository.GetAsync<List<Operation>>($"{BaseComboOperation}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        Operations = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedOperation = Operations!.Where(x => x.OperationId == Node.OperationId)
                    .Select(x => new Operation { OperationId = x.OperationId, OperationName = x.OperationName })
                    .FirstOrDefault();
        }
    }

    private void OperationChanged(Operation modelo)
    {
        Node.OperationId = modelo.OperationId;
        SelectedOperation = modelo;
    }

    private async Task LoadIpNetwork()
    {
        if (IsEditControl)
        {
            var responseHttp2 = await _repository.GetAsync<List<IpNetwork>>($"{BaseComboIpNetwork}/{Node.IpNetworkId}");
            bool errorHandler2 = await _responseHandler.HandleErrorAsync(responseHttp2);
            if (errorHandler2)
            {
                _navigationManager.NavigateTo($"{BaseView}");
                return;
            }
            IpNetworks = responseHttp2.Response;
            SelectedIpnetwork = IpNetworks!.Where(x => x.IpNetworkId == Node.IpNetworkId)
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
        Node.IpNetworkId = modelo.IpNetworkId;
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
            SelectedMark = Marks!.Where(x => x.MarkId == Node.MarkId)
                .Select(x => new Mark { MarkId = x.MarkId, MarkName = x.MarkName })
                .FirstOrDefault();

            await LoadMarkModel(SelectedMark!.MarkId);
        }
    }

    private async Task MarkChanged(Mark modelo)
    {
        Node.MarkId = modelo.MarkId;
        SelectedMark = modelo;
        SelectedMarkModel = new();
        await LoadMarkModel(Node.MarkId);
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
            SelectedMarkModel = MarkModels!.Where(x => x.MarkModelId == Node.MarkModelId)
                .Select(x => new MarkModel { MarkModelId = x.MarkModelId, MarkModelName = x.MarkModelName })
                .FirstOrDefault();
        }
    }

    private void MarkModelChanged(MarkModel modelo)
    {
        Node.MarkModelId = modelo.MarkModelId;
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
            SelectedSate = States!.Where(x => x.StateId == Node.StateId)
                .Select(x => new State { StateId = x.StateId, Name = x.Name })
                .FirstOrDefault();

            await LoadCity(SelectedSate!.StateId);
        }
    }

    private async Task StateChanged(State modelo)
    {
        Node.StateId = modelo.StateId;
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
            SelectedCity = Cities!.Where(x => x.CityId == Node.CityId)
                .Select(x => new City { CityId = x.CityId, Name = x.Name })
                .FirstOrDefault();
            await LoadZone(SelectedCity!.CityId);
        }
    }

    private async Task CityChanged(City modelo)
    {
        Node.CityId = modelo.CityId;
        SelectedCity = modelo;
        SelectedZone = new();
        await LoadZone(Node.CityId);
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
            SelectedZone = Zones!.Where(x => x.ZoneId == Node.ZoneId)
                .Select(x => new Zone { ZoneId = x.ZoneId, ZoneName = x.ZoneName })
                .FirstOrDefault();
        }
    }

    private void ZoneChanged(Zone modelo)
    {
        Node.ZoneId = modelo.ZoneId;
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