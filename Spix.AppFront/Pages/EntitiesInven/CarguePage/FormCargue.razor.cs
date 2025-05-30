using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Spix.AppFront.Helpers;
using Spix.Core.EntitesSoftSec;
using Spix.Core.EntitiesInven;
using Spix.HttpServices;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Spix.AppFront.Pages.EntitiesInven.CarguePage;

public partial class FormCargue
{
    private Usuario? SelectedUsuario;
    private List<Usuario>? Usuarios;

    private ProductStorage? SelectedProductStorage1;
    private List<ProductStorage>? ProductStorages1;

    private ProductStorage? SelectedProductStorage2;
    private List<ProductStorage>? ProductStorages2;

    private DateTime? DateMin = new DateTime(2024, 1, 1);
    private DateTime? DateStart = DateTime.Now;

    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    [Parameter, EditorRequired] public Cargue Cargue { get; set; } = null!;
    [Parameter, EditorRequired] public bool IsEditControl { get; set; }
    [Parameter, EditorRequired] public EventCallback OnSubmit { get; set; }
    [Parameter, EditorRequired] public EventCallback ReturnAction { get; set; }

    public bool FormPostedSuccessfully { get; set; } = false;

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