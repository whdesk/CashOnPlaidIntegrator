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
                var accountId = _config["PIBISI_ACCOUNT_ID"];
                if (string.IsNullOrEmpty(accountId))
                    return BadRequest(new { registrado = false, error = "No se configuró el ACCOUNT_ID de PIBISI." });

                // --- Paso 2: Proceder con el registro normal si no hay coincidencias peligrosas ---
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

                var findResponse = await _pibisiApi.FindSubjectAsync(accountId, formData, authToken);

                // Analizar matches muy cercanos y presencia en listas negras
                var dangerousMatches = new List<object>();
                bool matchCercano = false;
                bool enListaNegra = false;
                bool riesgoPorFlags = false;
                if (findResponse?.data?.matches != null && findResponse.data.matches.Count != 0)
                {
                    foreach (var match in findResponse.data.matches)
                    {
                        // Similaridad muy cercana
                        if (match.similarity >= 0.9)
                            matchCercano = true;

                        // Buscar en info si hay tipo de riesgo oficial
                        bool matchEnListaNegra = false;

                        // Evaluar flags de riesgo de forma tipada
                        var flagsObj = match.scoring;
                        bool isHighRisk = flagsObj?.is_high_risk ?? false;
                        bool isPep = flagsObj?.is_pep ?? false;
                        bool isSanctioned = flagsObj?.is_sanctioned ?? false;
                        bool isTerrorist = flagsObj?.is_terrorist ?? false;

                        if (isHighRisk || isPep || isSanctioned || isTerrorist)
                        {
                            riesgoPorFlags = true;
                        }

                        if (matchEnListaNegra || isHighRisk || isPep || isSanctioned || isTerrorist)
                        {
                            dangerousMatches.Add(new {
                                uuid = match.uuid,
                                similarity = match.similarity,
                                info = match.info?.Select(i => new { i.type, i.content }),
                                flags = flagsObj
                            });
                        }
                    }
                }

                if (matchCercano || enListaNegra || riesgoPorFlags)
                {
                    return Ok(new {
                        registrado = false,
                        coincidencias = dangerousMatches,
                        matchCercano,
                        enListaNegra,
                        riesgoPorFlags,
                        mensaje = "El cliente tiene coincidencias muy cercanas, en listas negras o presenta riesgo por flags. No se procederá con el registro."
                    });
                }

                // --- Paso 2: Proceder con el registro normal si no hay coincidencias peligrosas ---

                var response = await _pibisiApi.RegisterCustomer(accountId, formData);

                if (response?.data == null)
                    return BadRequest(new { registrado = false, error = "No se pudo registrar el cliente." });

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
