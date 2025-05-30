using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spix.AppFront.Helpers;
using Spix.AppFront.Pages.EntitiesInven.CarguePage;
using Spix.Core.EntitiesInven;
using Spix.HttpServices;

namespace Spix.AppFront.Pages.EntitiesInven.SerialPage;

public partial class IndexSerials
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IDialogService _dialogService { get; set; } = null!;
    [Inject] private SweetAlertService _SweetAlert { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    private int CurrentPage = 1;
    private int TotalPages;
    private int PageSize = 15;
    private const string baseUrl = "/api/v1/cargueDetails/GetSerials";

    public List<CargueDetail>? CargueDetails { get; set; }

    [Parameter] public Guid Id { get; set; }  //Codigo del CargueId
    private string Filter { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await Cargar();
    }

    private async Task SetFilterValue(string value)
    {
        Filter = value;
        await Cargar();
    }

    private async Task SelectedPage(int page)
    {
        CurrentPage = page;
        await Cargar(page);
    }

    private async Task Cargar(int page = 1)
    {
        var url = $"{baseUrl}?guidId={Id}&page={page}&recordsnumber={PageSize}";
        if (!string.IsNullOrWhiteSpace(Filter))
        {
            url += $"&filter={Filter}";
        }

        var responseHttp = await _repository.GetAsync<List<CargueDetail>>(url);
        // Centralizamos el manejo de errores
        bool errorHandled2 = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandled2)
        {
            _navigationManager.NavigateTo("/transfers");
            return;
        }

        TotalPages = int.Parse(responseHttp.HttpResponseMessage.Headers.GetValues("Totalpages").FirstOrDefault()!);

        CargueDetails = responseHttp.Response;
    }

    private async Task ShowModalAsync(Guid? id = null, bool isEdit = false)
    {
        var options = new DialogOptions() { CloseOnEscapeKey = true, CloseButton = true };
        IDialogReference? dialog;
        if (isEdit)
        {
            var parameters = new DialogParameters
            {
                { "Id", id }
            };
            //dialog = await _dialogService.ShowAsync<EditSellDetails>($"Editar Item", parameters, options);
            dialog = await _dialogService.ShowAsync<EditSerialsDetails>($"Editar Mac", parameters, options);
        }
        else
        {
            var parameters = new DialogParameters
            {
                { "Id", Id }
            };
            dialog = await _dialogService.ShowAsync<CreateCargueDetails>($"Nueva Mac", parameters, options);
        }

        var result = await dialog.Result;
        if (result!.Canceled)
        {
            await Cargar();
        }
    }
}