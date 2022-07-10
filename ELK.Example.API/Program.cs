using Elastic.CommonSchema.Serilog;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog((ctx, lc) => lc
        .WriteTo.Http(textFormatter: new EcsTextFormatter(), requestUri: "http://localhost:5000", queueLimitBytes: null)
        .Enrich.FromLogContext()
    );

var app = builder.Build();
app.MapGet("/", (Serilog.ILogger logger) =>
{
    logger.Error("this is an error");
    return "logged an error";
});
app.Run("http://localhost:3000");
