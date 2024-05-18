using MinimalWebAPI.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting...");

try
{

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.RegisterApplicationServices(builder.Configuration);

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApiDocument(config =>
    {
        config.DocumentName = "MinimalWebAPI";
        config.Title = "MinimalWebAPI v1";
        config.Version = "v1";
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseOpenApi();
        app.UseSwaggerUi(config =>
        {
            config.DocumentTitle = "MinimalWebAPI";
            config.Path = "/swagger";
            config.DocumentPath = "/swagger/{documentName}/swagger.json";
            config.DocExpansion = "list";
        });
        app.MapGet("/", () =>
        {
            return Results.Redirect("/swagger");
        });
    }

    app.MapEndpoints();

    await app.RunAsync();

    Log.Information("Application stopped without errors");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}