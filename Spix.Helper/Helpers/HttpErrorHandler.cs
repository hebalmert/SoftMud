using Microsoft.EntityFrameworkCore;
using Spix.CoreShared.Responses;

namespace Spix.Helper.Helpers;

public class HttpErrorHandler
{
    public async Task<ActionResponse<T>> HandleErrorAsync<T>(Exception exception)
    {
        string errorMessage = "Error desconocido.";

        // Manejo de errores HTTP
        if (exception is HttpRequestException httpEx)
        {
            errorMessage = $"Error en la solicitud HTTP: {httpEx.Message}";
            return new ActionResponse<T> { WasSuccess = false, Message = errorMessage, Result = default };
        }

        // Manejo de errores de Base de Datos
        if (exception is DbUpdateException dbEx)
        {
            if (dbEx.InnerException?.Message.Contains("duplicate key") == true ||
                dbEx.InnerException?.Message.Contains("UNIQUE constraint") == true)
            {
                errorMessage = "Error: El registro ya existe en la base de datos.";
            }
            else if (dbEx.InnerException?.Message.Contains("foreign key constraint") == true ||
                     dbEx.InnerException?.Message.Contains("REFERENCE") == true)
            {
                errorMessage = "Error: No se puede eliminar el registro porque está referenciado en otra tabla.";
            }
        }
        else if (exception is DbUpdateConcurrencyException)
        {
            errorMessage = "Error: Conflicto de concurrencia. Otro usuario modificó el registro antes que tú.";
        }

        return new ActionResponse<T>
        {
            WasSuccess = false,
            Message = errorMessage,
            Result = default
        };
    }
}