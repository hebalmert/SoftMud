using Microsoft.AspNetCore.Components;
using Spix.Core.EntitiesContratos;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Spix.AppFront.Pages.EntitiesContratos.ContractControlPage;

public partial class DetailControlContrato
{
    [Parameter, EditorRequired] public ContractClient model { get; set; } = null!;

    [Parameter] public Guid Id { get; set; }

    protected override void OnInitialized()
    {
        model = new();
        model.StateType = CoreShared.Enum.StateType.Procesando;
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