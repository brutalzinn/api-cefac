using Microsoft.AspNetCore.Mvc;

namespace CefacAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanilhaController : ControllerBase
    {
        private readonly ILogger<PlanilhaController> _logger;

        public PlanilhaController(ILogger<PlanilhaController> logger)
        {
            _logger = logger;
        }
        public class TestePayload
        {
            public string Nome { get; set; }
            public DateTime DataNascimento { get; set; }
            public string Email { get; set; }
        }


        [HttpPost]
        public IActionResult AtualizarPlanilha([FromBody] TestePayload request)
        {
            _logger.LogInformation("LOG {log}", System.Text.Json.JsonSerializer.Serialize(request));

            Logger.Log("LOG {0}", System.Text.Json.JsonSerializer.Serialize(request));
            return Ok("OK");
        }
    }
}