using Spix.CoreShared.Responses;
using Spix.CoreShared.ResponsesSec;

namespace Spix.Helper;

public interface IEmailHelper
{
    //Sistema para el envio de Correos Electronicos
    Task<bool> EnviarAsync(ContactViewDTO contacto);

    //Sistema para Confirmar las Cuentas de Usuario desde el Correo
    Task<Response> ConfirmarCuenta(string to, string NameCliente, string subject, string body);
}