using Elastic.CommonSchema.Serilog;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();

builder.Host.UseSerilog(( _, config) => config
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.DurableHttpUsingFileSizeRolledBuffers(textFormatter: new EcsTextFormatter(), requestUri: builder.Configuration.GetConnectionString("Logstash"))
    .Enrich.FromLogContext());

var app = builder.Build();

Random random = new Random();
int secretValue = random.Next(1, 100000);
app.MapGet("/{id:int}", (Serilog.ILogger logger, int id) =>
{
    if (id == secretValue)
    {
        logger.Error("You found the secret value {id}", id);
        return Results.Ok($"{id} is the secret value");
    }
    logger.Warning("You have not found the secret value {id}", id);
    return Results.BadRequest($"{id} is not the secret value");
});
app.Run();
