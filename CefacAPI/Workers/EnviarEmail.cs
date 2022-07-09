using CefacAPI.Workers.Models;
using CronInterpreter;

namespace WorkerSample;

public class EnviarEmail : BackgroundService
{
    private readonly ILogger<EnviarEmail> _logger;
    private readonly WorkerResource _resource;

    public EnviarEmail(ILogger<EnviarEmail> logger, IEnumerable<WorkerResource> resourceList)
    {
        _logger = logger;
        _resource = resourceList.First(e => e.Nome.Equals("EnviarEmail"));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && _resource.Ativo)
        {
            _logger.LogInformation("Ativo: {status}, EnviarEmail rodando em: {time} Expression: {resource}", _resource.Ativo, DateTime.Now, _resource.CronExpression);
           
            var cronExpression = new CronJob(_resource.CronExpression, DateTime.Now);

            var atual = DateTime.Now.CreateWithoutSeconds();
            //AQUI VOU PEGAR A LISTA DOS ANIVERSARIANTES E  VOU ENVIAR UM EMAIL PRA CADA UM.

            if (atual == cronExpression.NovoDateTime)
            {
                _logger.LogInformation("Enviando email em", DateTime.Now);
                cronExpression = new CronJob(_resource.CronExpression, DateTime.Now);
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}
