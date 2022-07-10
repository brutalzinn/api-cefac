using CefacAPI.Messages;
using CefacAPI.Services;
using CefacAPI.Workers.Models;
using CronInterpreter;

namespace WorkerSample;

public class EnviarEmailWorker : BackgroundService
{
    private readonly ILogger<EnviarEmailWorker> _logger;
    private readonly WorkerResource _resource;
    private readonly IRedisService _redisService;
    private CronJob _cronJob;


    public EnviarEmailWorker(ILogger<EnviarEmailWorker> logger, IRedisService redisService, WorkerResource resource)
    {
        _logger = logger;
        _redisService = redisService;
        _resource = resource;
        _cronJob = new CronJob(_resource.CronExpression, DateTime.Now);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && _resource.Ativo)
        {
            _logger.LogInformation("Ativo: {status}, EnviarEmail rodando em: {time} Expression: {resource}", _resource.Ativo, DateTime.Now, _resource.CronExpression);
           
            //AQUI VOU PEGAR A LISTA DOS ANIVERSARIANTES E  VOU ENVIAR UM EMAIL PRA CADA UM.
            if (_cronJob.IsDispatchTime())
            {
                var aniversariantes = _redisService.Get<IEnumerable<DadosTabelaPayload>>("tabela");
                
                foreach(var data in aniversariantes)
                {
                    _logger.LogInformation("Enviando email para {0}", data.Email);
                }

                _logger.LogInformation("Emails enviados {0}", aniversariantes.Count());
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
