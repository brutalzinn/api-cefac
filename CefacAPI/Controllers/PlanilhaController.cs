using CefacAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CefacAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanilhaController : ControllerBase
    {
        private readonly ILogger<PlanilhaController> _logger;
        private readonly IRedisService _redisService;

        public PlanilhaController(ILogger<PlanilhaController> logger, IRedisService redisService)
        {
            _logger = logger;
            _redisService = redisService;
        }

        public class TestePayload
        {
            public string Nome { get; set; }
            public DateTime DataNascimento { get; set; }
            public string Email { get; set; }
        }

        //planilha do google faz um post aqui

        [HttpGet("ObterAniversariantesDoMes")]
        public IActionResult ObterAniversariantesDoMes([FromQuery] int? mes)
        {
            var listaAniversariantes = _redisService.Get<IEnumerable<TestePayload>>("tabela");

            listaAniversariantes = listaAniversariantes.Where(x => x.DataNascimento.Month.Equals(mes.GetValueOrDefault(DateTime.Now.Month))).ToList();
            
            return Ok(listaAniversariantes);
        }

        [HttpGet("ObterAniversariantesProximoMes")]
        public IActionResult ObterAniversariantesProximoMes()
        {
            var listaAniversariantes = _redisService.Get<IEnumerable<TestePayload>>("tabela");
            var timeAtual = DateTime.Now;
            listaAniversariantes = listaAniversariantes.Where(x =>  x.DataNascimento.Month >= timeAtual.Date.Month).ToList();

            return Ok(listaAniversariantes);
        }

        [HttpGet("ObterAniversariantes")]
        public IActionResult ObterAniversariantes()
        {
            var listaAniversariantes = _redisService.Get<IEnumerable<TestePayload>>("tabela");

            return Ok(listaAniversariantes);
        }

        [HttpPost]
        public IActionResult AtualizarPlanilha([FromBody] IEnumerable<TestePayload> request)
        {
            var requestJson = System.Text.Json.JsonSerializer.Serialize(request);
            _logger.LogInformation("LOG {log}", requestJson);
            Logger.Log("LOG {0}", requestJson);
            _redisService.Set("tabela", request);
            return new StatusCodeResult(204);
        }
    }
}