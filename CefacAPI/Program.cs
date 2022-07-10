using CefacAPI.Config.Provider;
using CefacAPI.Services;
using CefacAPI.Workers.Models;
using WorkerSample;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json").Build();

var section = config.GetSection("Workers");
var workers = section.Get<IEnumerable<WorkerResource>>();


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        config.GetConnectionString("Redis");
});

builder.Services.AddSingleton<IRedisService, RedisService>();

builder.Services.AddSingleton<IHostedService>(service => {

    var resource = workers.First(e => e.Nome.Equals("EnviarEmail"));
    var logger = service.GetRequiredService<ILogger<EnviarEmailWorker>>();
    var redisService = service.GetRequiredService<IRedisService>();

    return new EnviarEmailWorker(logger, redisService, resource);
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options =>
{
    options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
});
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.Run();
await app.RunAsync();


//await builder..RunAsync();