using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Spix.AppFront.Helpers;
using Spix.Core.EntitiesGen;
using Spix.Core.EntitiesOper;
using Spix.HttpServices;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Spix.AppFront.Pages.EntitiesOper.ClientPage;

public partial class FormClient
{
    [Inject] private SweetAlertService _sweetAlert { get; set; } = null!;
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    private DocumentType? SelectedDocument;
    private List<DocumentType>? DocumentTypes;
    private string? ImageUrl;
    private string BaseView = "/clients";
    private string BaseComboDocument = "/api/v1/documenttypes/loadCombo";

    [Parameter, EditorRequired] public Client Client { get; set; } = null!;
    [Parameter, EditorRequired] public EventCallback OnSubmit { get; set; }
    [Parameter, EditorRequired] public EventCallback ReturnAction { get; set; }
    [Parameter, EditorRequired] public bool IsEditControl { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await LoadDocument();
        if (IsEditControl)
        {
            ImageUrl = Client.ImageFullPath;
        }
    }

    private async Task LoadDocument()
    {
        var responseHttp = await _repository.GetAsync<List<DocumentType>>($"{BaseComboDocument}");
        bool errorHandler = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandler)
        {
            _navigationManager.NavigateTo($"{BaseView}");
            return;
        }
        DocumentTypes = responseHttp.Response;
        if (IsEditControl)
        {
            SelectedDocument = DocumentTypes!.Where(x => x.CorporationId == Client.CorporationId)
                .Select(x => new DocumentType { DocumentTypeId = x.DocumentTypeId, DocumentName = x.DocumentName })
                .FirstOrDefault();
        }
    }

    private void DocumentChanged(DocumentType modelo)
    {
        Client.DocumentTypeId = modelo.DocumentTypeId;
        SelectedDocument = modelo;
    }

    private void ImageSelected(string imagenBase64)
    {
        Client.ImgBase64 = imagenBase64;
        ImageUrl = null;
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