using Elastic.CommonSchema.Serilog;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog(( _, config) => config
        .MinimumLevel.Warning()
        .WriteTo.DurableHttpUsingFileSizeRolledBuffers(textFormatter: new EcsTextFormatter(), requestUri: "http://logstash:8080")
        .WriteTo.Console()
        .Enrich.FromLogContext()
    );

var app = builder.Build();

Random random = new Random();
int secretValue = random.Next(1, 1000000);
app.MapGet("/{id:int}", (Serilog.ILogger logger, int id) =>
{
    if (id == secretValue)
    {
        logger.Error("You found the secret value {id}", id);
    }

    return Results.Ok($"{id} is not the secret value");
});
app.Run();
