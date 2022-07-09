
// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
//https://www.infoworld.com/article/3662294/use-logging-and-di-in-minimal-apis-in-aspnet-core-6.html

using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host
    .UseSerilog((ctx, lc) => lc
        .WriteTo.Http(requestUri: "http://localhost:5000", queueLimitBytes: null)
        .Enrich.FromLogContext()
    );

var app = builder.Build();
app.MapGet("/", (Serilog.ILogger logger) =>
{
    logger.Error("this is an error");
    return "logged an error";
});
app.Run("http://localhost:3000");
