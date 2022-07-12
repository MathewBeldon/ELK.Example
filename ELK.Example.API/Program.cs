using Elastic.CommonSchema.Serilog;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog(( _, config) => config
        .WriteTo.DurableHttpUsingFileSizeRolledBuffers(textFormatter: new EcsTextFormatter(), requestUri: "http://localhost:8080")
        .Enrich.FromLogContext()
    );

var app = builder.Build();
app.MapGet("/{id:int}", (Serilog.ILogger logger, int id) =>
{
    logger.Error(new NullReferenceException("This was not found"), "this is an error {id}", id);
    return Results.Ok($"logged an error {id}"); 
});
app.Run("http://localhost:3000");
