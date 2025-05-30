using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Spix.AppFront.Helpers;
using Spix.Core.EntitiesInven;
using Spix.HttpServices;

namespace Spix.AppFront.Pages.EntitiesInven.CarguePage;

public partial class DetailsCargueDetails
{
    [Inject] private IRepository _repository { get; set; } = null!;
    [Inject] private NavigationManager _navigationManager { get; set; } = null!;
    [Inject] private IDialogService _dialogService { get; set; } = null!;
    [Inject] private SweetAlertService _SweetAlert { get; set; } = null!;
    [Inject] private HttpResponseHandler _responseHandler { get; set; } = null!;

    private int CurrentPage = 1;
    private int TotalPages;
    private int PageSize = 15;
    private const string baseUrl = "/api/v1/cargueDetails";

    public Cargue? Cargue { get; set; }
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
        var responseHttpCountry = await _repository.GetAsync<Cargue>($"/api/v1/cargues/{Id}");
        // Centralizamos el manejo de errores
        bool errorHandled = await _responseHandler.HandleErrorAsync(responseHttpCountry);
        if (errorHandled)
        {
            _navigationManager.NavigateTo("/transfers");
            return;
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

        Cargue = responseHttpCountry.Response;
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
            dialog = await _dialogService.ShowAsync<EditCargueDetails>($"Editar Mac", parameters, options);
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

    private async Task CloseCargueAsync(Guid id)
    {
        var result = await _SweetAlert.FireAsync(new SweetAlertOptions
        {
            Title = "Desea Cerrar Tranferencia",
            Text = "¿Al Cerrar la Transferencia, no podra volver editar y los Inventarios se actualizaran, Continuar?",
            Icon = SweetAlertIcon.Question,
            ShowCancelButton = true,
            CancelButtonText = "No",
            ConfirmButtonText = "Si"
        });

        var confirm = string.IsNullOrEmpty(result.Value);
        if (confirm)
        {
            return;
        }

        var responseHttp = await _repository.GetAsync($"{baseUrl}/CerrarTrans/{Cargue!.CargueId}");
        // Centralizamos el manejo de errores
        bool errorHandled = await _responseHandler.HandleErrorAsync(responseHttp);
        if (errorHandled)
        {
            _navigationManager.NavigateTo("/cargues");
            return;
        }

        await Cargar();
    }

    private async Task DeleteAsync(Guid id)
    {
        var result = await _SweetAlert.FireAsync(new SweetAlertOptions
        {
            Title = "Confirmación",
            Text = "¿Realmente deseas eliminar el registro?",
            Icon = SweetAlertIcon.Question,
            ShowCancelButton = true,
            CancelButtonText = "No",
            ConfirmButtonText = "Si"
        });

        var confirm = string.IsNullOrEmpty(result.Value);
        if (confirm)
        {
            return;
        }

        var responseHTTP = await _repository.DeleteAsync($"{baseUrl}/{id}");
        // Centralizamos el manejo de errores
        bool errorHandled = await _responseHandler.HandleErrorAsync(responseHTTP);
        if (errorHandled)
        {
            await Cargar();
            return;
        }
        await Cargar();
    }
}