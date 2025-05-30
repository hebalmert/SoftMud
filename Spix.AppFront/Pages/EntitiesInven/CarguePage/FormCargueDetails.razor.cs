using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Spix.AppFront.Helpers;
using Spix.Core.EntitiesInven;
using Spix.CoreShared.Enum;
using Spix.HttpServices;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Spix.AppFront.Pages.EntitiesInven.CarguePage;

public partial class FormCargueDetails
{
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    private EnumItemModel? SelectedUserType;
    private List<EnumItemModel>? ListUserType;

    [Parameter, EditorRequired] public CargueDetail CargueDetail { get; set; } = null!;
    [Parameter, EditorRequired] public bool IsEditControl { get; set; }
    [Parameter, EditorRequired] public EventCallback OnSubmit { get; set; }
    [Parameter, EditorRequired] public EventCallback ReturnAction { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadStates();
    }

    private async Task LoadStates()
    {
        var responseHTTP = await _repository.GetAsync<List<EnumItemModel>>($"api/v1/cargues/loadComboStatus");
        bool errorHandled = await _responseHandler.HandleErrorAsync(responseHTTP);
        if (errorHandled)
        {
            _navigationManager.NavigateTo("/usuarios");
            return;
        }
        ListUserType = responseHTTP.Response;
        if (IsEditControl)
        {
            SelectedUserType = ListUserType!.Where(x => x.Name == CargueDetail.Status.ToString())
                .Select(x => new EnumItemModel { Name = x.Name, Value = x.Value }).FirstOrDefault();
        }
        else
        {
            SelectedUserType = ListUserType!.Where(x => x.Name == SerialStateType.Disponible.ToString())
                .Select(x => new EnumItemModel { Name = x.Name, Value = x.Value }).FirstOrDefault();
        }
    }

    private void UsertTypeChanged(EnumItemModel modelo)
    {
        if (modelo.Name == "Disponible") { CargueDetail.Status = SerialStateType.Disponible; }
        if (modelo.Name == "Averiado") { CargueDetail.Status = SerialStateType.Averiado; }
        if (modelo.Name == "Operativo") { CargueDetail.Status = SerialStateType.Operativo; }
        SelectedUserType = modelo;
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