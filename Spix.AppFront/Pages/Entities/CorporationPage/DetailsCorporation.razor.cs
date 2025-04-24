using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Spix.AppFront.Helpers;
using Spix.Core.Entities;
using Spix.HttpServices;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Spix.AppFront.Pages.Entities.CorporationPage;

public partial class DetailsCorporation
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    private Corporation? Corporation;
    private SoftPlan? SoftPlan;
    private Country? Country;

    [Parameter] public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadCorporation();
        await LoadSoftPlans();
        await LoadCountry();
    }

    private async Task LoadCorporation()
    {
        var responseHTTP = await _repository.GetAsync<Corporation>($"/api/v1/corporations/{Id}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHTTP);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"/corporations");
            return;
        }
        Corporation = responseHTTP.Response;
    }

    private async Task LoadSoftPlans()
    {
        var responseHTTP = await _repository.GetAsync<SoftPlan>($"/api/v1/softplans/{Corporation!.SoftPlanId}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHTTP);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"/corporations");
            return;
        }
        SoftPlan = responseHTTP.Response;
    }

    private async Task LoadCountry()
    {
        var responseHTTP = await _repository.GetAsync<Country>($"/api/v1/countries/{Corporation!.CountryId}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHTTP);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"/corporations");
            return;
        }
        Country = responseHTTP.Response;
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