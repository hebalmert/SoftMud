using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spix.AppFront.Helpers;
using Spix.Core.Entities;
using Spix.Core.EntitiesContratos;
using Spix.Core.EntitiesGen;
using Spix.Core.EntitiesOper;
using Spix.HttpServices;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Spix.AppFront.Pages.EntitiesContratos.ContractClientPage;

public partial class FormContractClient
{
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    [Parameter, EditorRequired] public ContractClient ContractClient { get; set; } = null!;
    [Parameter, EditorRequired] public EventCallback OnSubmit { get; set; }
    [Parameter, EditorRequired] public EventCallback ReturnAction { get; set; }
    [Parameter] public EventCallback PrecesarAction { get; set; }
    [Parameter, EditorRequired] public bool IsEditControl { get; set; }

    private State? SelectedSate;
    private List<State>? States;
    private City? SelectedCity = new();
    private List<City>? Cities = new();
    private Zone? SelectedZone = new();
    private List<Zone>? Zones = new();

    private Contractor? SelectedContractor = new();
    private List<Contractor>? Contractors = new();

    private Client? SelectedClient = new();
    private List<Client>? Clients = new();

    private ServiceCategory? SelectedServiceCategory = new();
    private List<ServiceCategory>? ServiceCategories = new();

    private ServiceClient? SelectedServiceClient = new();
    private List<ServiceClient>? ServiceClients = new();

    private string BaseView = "/contractclients";
    private string BaseComboState = "/api/v1/states/loadCombo";
    private string BaseComboCity = "/api/v1/cities/loadCombo";
    private string BaseComboZone = "/api/v1/zones/loadCombo";
    private string BaseComboClient = "/api/v1/clients/loadCombo";
    private string BaseComboContractor = "/api/v1/contractor/loadCombo";
    private string BaseComboServiceCategory = "/api/v1/servicecategories/loadCombo";
    private string BaseComboServiceClient = "/api/v1/serviceclients/loadCombo";

    protected override async Task OnInitializedAsync()
    {
        await LoadState();
        await LoadContractor();
        await LoadClient();
        await LoadServiceCategory();
    }

    private async Task LoadServiceCategory()
    {
        var responseHttp = await _repository.GetAsync<List<ServiceCategory>>($"{BaseComboServiceCategory}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        ServiceCategories = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedServiceCategory = ServiceCategories!.Where(x => x.ServiceCategoryId == ContractClient.ServiceCategoryId)
                .Select(x => new ServiceCategory { ServiceCategoryId = x.ServiceCategoryId, Name = x.Name })
                .FirstOrDefault();

            await LoadServiceClient(SelectedServiceCategory!.ServiceCategoryId);
        }
    }

    private async Task ServiceCategoryChanged(ServiceCategory modelo)
    {
        ContractClient.ServiceCategoryId = modelo.ServiceCategoryId;
        SelectedServiceCategory = modelo;
        SelectedServiceClient = new();
        await LoadServiceClient(ContractClient.ServiceCategoryId);
    }

    private async Task LoadServiceClient(Guid id)
    {
        var responseHttp = await _repository.GetAsync<List<ServiceClient>>($"{BaseComboServiceClient}/{id}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        ServiceClients = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedServiceClient = ServiceClients!.Where(x => x.ServiceClientId == ContractClient.ServiceClientId)
                .Select(x => new ServiceClient { ServiceClientId = x.ServiceClientId, ServiceName = x.ServiceName })
                .FirstOrDefault();
        }
    }

    private async Task ServiceClientChanged(ServiceClient modelo)
    {
        ContractClient.ServiceClientId = modelo.ServiceClientId;
        SelectedServiceClient = modelo;
        var responseHttp = await _repository.GetAsync<ServiceClient>($"api/v1/serviceclients/{modelo.ServiceClientId}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        modelo = responseHttp.Response!;
        ContractClient.ServiceName = modelo.ServiceName;
        ContractClient.Impuesto = modelo.Tax!.Rate;
        ContractClient.Price = modelo.Price;
    }

    private async Task<IEnumerable<Client>> SearchCountry(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return Clients!;
        }

        return Clients!
            .Where(x => x.FullName!.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    private async Task LoadContractor()
    {
        var responseHttp = await _repository.GetAsync<List<Contractor>>($"{BaseComboContractor}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        Contractors = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedContractor = Contractors!.Where(x => x.ContractorId == ContractClient.ContractorId)
                .Select(x => new Contractor { ContractorId = x.ContractorId, FullName = x.FullName })
                .FirstOrDefault();
        }
    }

    private void ContractorChanged(Contractor modelo)
    {
        ContractClient.ContractorId = modelo.ContractorId;
        SelectedContractor = modelo;
    }

    private async Task LoadClient()
    {
        var responseHttp = await _repository.GetAsync<List<Client>>($"{BaseComboClient}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        Clients = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedClient = Clients!.Where(x => x.ClientId == ContractClient.ClientId)
                .Select(x => new Client { ClientId = x.ClientId, FullName = x.FullName })
                .FirstOrDefault();
        }
    }

    private async Task ClientChanged(Client modelo)
    {
        ContractClient.ClientId = modelo.ClientId;
        SelectedClient = modelo;
        var responseHttp = await _repository.GetAsync<Client>($"/api/v1/clients/{modelo.ClientId}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        modelo = responseHttp.Response!;
        ContractClient.CodeCountry = modelo.CodeCountry;
        ContractClient.CodeNumber = modelo.CodeNumber;
        ContractClient.PhoneNumber = modelo.PhoneNumber;
        ContractClient.Address = modelo.Address;
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
            SelectedSate = States!.Where(x => x.StateId == ContractClient.StateId)
                .Select(x => new State { StateId = x.StateId, Name = x.Name })
                .FirstOrDefault();

            await LoadCity(SelectedSate!.StateId);
        }
    }

    private async Task StateChanged(State modelo)
    {
        ContractClient.StateId = modelo.StateId;
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
            SelectedCity = Cities!.Where(x => x.CityId == ContractClient.CityId)
                .Select(x => new City { CityId = x.CityId, Name = x.Name })
                .FirstOrDefault();
            await LoadZone(SelectedCity!.CityId);
        }
    }

    private async Task CityChanged(City modelo)
    {
        ContractClient.CityId = modelo.CityId;
        SelectedCity = modelo;
        SelectedZone = new();
        await LoadZone(ContractClient.CityId);
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
            SelectedZone = Zones!.Where(x => x.ZoneId == ContractClient.ZoneId)
                .Select(x => new Zone { ZoneId = x.ZoneId, ZoneName = x.ZoneName })
                .FirstOrDefault();
        }
    }

    private void ZoneChanged(Zone modelo)
    {
        ContractClient.ZoneId = modelo.ZoneId;
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