using Microsoft.AspNetCore.Mvc;
using PlaidApi.Pibisi;

namespace PlaidApi.Controllers
{
    [ApiController]
    [Route("api/pibisi")]
    public class PibisiController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IPibisiApi _pibisiApi;
        public PibisiController(IConfiguration config, IPibisiApi pibisiApi)
        {
            _config = config;
            _pibisiApi = pibisiApi;
        }

        [HttpGet("customer/{customer}")]
        public async Task<IActionResult> GetCustomer(string customer)
        {
            var accountId = _config["PIBISI_ACCOUNT_ID"];
            if (string.IsNullOrWhiteSpace(accountId))
                return BadRequest(new { error = "PIBISI_ACCOUNT_ID no configurado" });
            try
            {
                var result = await _pibisiApi.GetCustomer(accountId, customer);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("validar-cliente")]
        public async Task<IActionResult> ValidarCliente([FromBody] PibisiCustomerBasicRequest basicReq)
        {
            // Mapear de basicReq a PibisiCustomerRequest
            var req = new PibisiCustomerRequest
            {
                PersonType = "P",
                FullName = basicReq.FullName,
                NationalId = basicReq.NationalId,
                NationalIdCountry = basicReq.Country,
                BirthDate = basicReq.BirthDate,
            };

            var authToken = _config["PIBISI_AUTH_TOKEN"];
            if (string.IsNullOrWhiteSpace(authToken))
            {
                throw new InvalidOperationException("El token de autenticación de PIBISI no está definido en los secretos de usuario ni en la configuración.");
            }

            try
            {
                // Usar HttpClient inyectado por DI para enviar form-urlencoded
                var accountId = _config["PIBISI_ACCOUNT_ID"];
                if (string.IsNullOrEmpty(accountId))
                    return BadRequest(new { registrado = false, error = "No se configuró el ACCOUNT_ID de PIBISI." });

                // Construir el arreglo de POIs a partir del request
                // Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(req.PersonType) ||
                    string.IsNullOrWhiteSpace(req.FullName) ||
                    string.IsNullOrWhiteSpace(req.BirthDate))
                {
                    return BadRequest(new { registrado = false, error = "Campos obligatorios vacíos: person, name.full, birth.date" });
                }

                // Solo agregar el identificador nacional si está presente
                var idPois = new List<object>();
                if (!string.IsNullOrWhiteSpace(req.NationalId) && !string.IsNullOrWhiteSpace(req.NationalIdCountry))
                    idPois.Add(new { type = "id.national", content = new { number = req.NationalId, country = req.NationalIdCountry } });

                // Construir y filtrar POIs (solo los que tengan contenido)
                var pois = new List<object>
                {
                    new { type = "person", content = req.PersonType },
                    new { type = "name.full", content = req.FullName },
                    new { type = "birth.date", content = req.BirthDate },
                    new { type = "email", content = req.Email },
                }
                .Where(p => !string.IsNullOrWhiteSpace((string)p.GetType().GetProperty("content").GetValue(p)))
                .ToList();

                // Agregar POIs adicionales
                if (idPois.Count > 0)
                    pois.AddRange(idPois);

                var poisJson = System.Text.Json.JsonSerializer.Serialize(pois);

                var formData = new PibisiCustomerFormData { Pois = poisJson };
                var response = await _pibisiApi.RegisterCustomer(accountId, formData);

                // Validar si está en listas negras (simulación: buscar campo "blacklist" en data/meta)
                bool enListaNegra = false;
                if (response.data != null && response.data.ToString().ToLower().Contains("blacklist"))
                    enListaNegra = true;
                if (response.meta != null && response.meta.ToString().ToLower().Contains("blacklist"))
                    enListaNegra = true;

                // Armar respuesta simplificada y útil para frontend
                var flags = response.data?.scoring?.flags;
                var scoringValue = response.data?.scoring?.value ?? 0;
                bool esAltoRiesgo = (flags?.is_high_risk ?? false) || (flags?.is_pep ?? false) || (flags?.is_sanctioned ?? false) || (flags?.is_terrorist ?? false) || scoringValue >= 70;
                bool esPEP = flags?.is_pep ?? false;
                bool esSancionado = flags?.is_sanctioned ?? false;
                bool esTerrorista = flags?.is_terrorist ?? false;
                bool riesgoAltoScoring = scoringValue >= 70;

                return Ok(new
                {
                    registrado = true,
                    uuid = response.data?.uuid,
                    status = response.data?.status,
                    risk = response.data?.risk,
                    scoring = scoringValue,
                    flags = flags,
                    esAltoRiesgo = esAltoRiesgo,
                    esPEP = esPEP,
                    esSancionado = esSancionado,
                    esTerrorista = esTerrorista,
                    riesgoAltoScoring = riesgoAltoScoring,
                    info = response.data?.info?.Select(i => new
                    {
                        type = i.type,
                        content = i.content
                    })
                });

            }
            catch (Exception ex)
            {
                return BadRequest(new { registrado = false, error = ex.Message });
            }
        }

    }
}
