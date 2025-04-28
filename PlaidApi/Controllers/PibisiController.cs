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
            // TODO: Reemplaza estos valores por los reales o usa IConfiguration
            var authToken = "";

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
