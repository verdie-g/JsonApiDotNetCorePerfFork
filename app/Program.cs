using System.Text.Json.Serialization;
using App;
using App.Data;
using App.Definitions;
using JsonApiDotNetCore.Configuration;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "HH:mm:ss ";
});

var connectionString = builder.Configuration.GetConnectionString("Default")!;
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddJsonApi<AppDbContext>(options =>
{
    options.UseRelativeLinks = true;
    options.IncludeTotalResourceCount = true;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddResourceDefinition<TodoItemDefinition>();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(b => b.AddService("app"))
    .WithMetrics(b =>
    {
        b.AddMeter("*")
            .AddProcessInstrumentation()
            .AddRuntimeInstrumentation()
            .AddOtlpExporter((exporterOptions, metricReaderOptions) =>
            {
                exporterOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                exporterOptions.Endpoint = new Uri("http://localhost:8080/otlp/v1/metrics");
                // 15 seconds is the default scrape interval of a prometheus data source in Grafana.
                metricReaderOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 15_000;
            });
    });

builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await Seeder.EnsureSampleDataAsync(dbContext);
}

app.UseRouting();
app.UseJsonApi();
app.MapControllers();

app.Run();