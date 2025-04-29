using Microsoft.AspNetCore.Mvc;
using PlaidApi.Pibisi;
using Refit;
using System.Threading.Tasks;

namespace PlaidApi.Controllers
{
    [ApiController]
    [Route("api/pibisi")]
    public class PibisiController : ControllerBase
    {
        [HttpPost("validar-cliente")]
        public async Task<IActionResult> ValidarCliente([FromBody] PibisiCustomerRequest req)
        {
            // Obtener el token de autenticación desde una variable de entorno
            var authToken = Environment.GetEnvironmentVariable("PIBISI_AUTH_TOKEN");
            if (string.IsNullOrWhiteSpace(authToken))
            {
                throw new InvalidOperationException("El token de autenticación de PIBISI no está definido en la variable de entorno PIBISI_AUTH_TOKEN.");
            }

            try
            {
                var pibisiApi = RestService.For<IPibisiApi>("https://int.api.pibisi.com/v2");

                var redentials = await pibisiApi.GetCredentials(authToken);

                var uuid = await pibisiApi.GetAccounts(authToken);
                //var response = await pibisiApi.RegisterCustomer(
                //    authToken,
                //    req.PersonType,
                //    req.FullName,
                //    req.NationalId,
                //    req.BirthDate
                // Agrega otros campos si lo necesitas
                //); 
                
                return Ok();


            }
            catch (Exception ex)
            {
               
                throw;
            }

          
        }
    }
}
