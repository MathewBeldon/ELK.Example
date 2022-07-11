using Elastic.CommonSchema.Serilog;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog(( _, config) => config
        .WriteTo.Http(textFormatter: new EcsTextFormatter(), requestUri: "http://localhost:5000", queueLimitBytes: null)
        .Enrich.FromLogContext()
    );

var app = builder.Build();
app.MapGet("/{id:int}", (Serilog.ILogger logger, int id) =>
{
    logger.Error("this is an error {id}", id);
    return Results.Ok($"logged an error {id}"); 
});
app.Run("http://localhost:3000");
