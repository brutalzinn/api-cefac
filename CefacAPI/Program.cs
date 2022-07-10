using CefacAPI.Config.Provider;
using CefacAPI.Workers.Models;
using WorkerSample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json").Build();

var section = config.GetSection("Workers");
var workers = section.Get<IEnumerable<WorkerResource>>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHostedService>(service => new EnviarEmail(service.GetService<ILogger<EnviarEmail>>(), workers));
builder.Services.AddMvc(options =>
{
    options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
});
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