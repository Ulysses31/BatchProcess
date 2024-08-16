using System.Diagnostics;
using System.Reflection;
using BatchProcess.Api.Database;
using BatchProcess.Api.Repository;
using BatchProcess.API;
using BatchProcess.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Core;

var builder = WebApplication.CreateBuilder(args);
var envName = builder.Environment.EnvironmentName;
var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "BatchProcess API",
            Version = "v1",
            Description = "BatchProcess API",
            Contact = new OpenApiContact
            {
                Name = "Iordanidis Chris",
                Email = "iordanidischr@gmail.com"
            },
            License = new OpenApiLicense
            {
                Name = "MIT",
                Url = new Uri("https://opensource.org/licenses/MIT")
            },
            TermsOfService = new Uri("https://opensource.org/licenses/MIT"),
            Extensions = new Dictionary<string, IOpenApiExtension>
            {
                { "x-logo", new OpenApiObject
                    {
                        { "url", new OpenApiString("https://avatars.githubusercontent.com/u/8908511?s=200&v=4") },
                        { "backgroundColor", new OpenApiString("#000") },
                        { "altText", new OpenApiString("BatchProcess") }
                    }
                }
            }
        }
    );
});

//******* Db Context *******//
#region DbContext
builder.Services.AddDbContext<BatchProcessDbContext>(options =>
{
    // options.UseSqlServer(
    //     $"Server={server},{port};Database={database};User={user};Password={password};Encrypt=Optional;TrustServerCertificate=True"
    // )
    options.UseSqlite(
        "Data Source=BatchProcess.db",
        o => o.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName)
    )
    .LogTo(
        message => Console.WriteLine(message),
        envName == "Development" ? LogLevel.Trace : LogLevel.Error,
        DbContextLoggerOptions.DefaultWithUtcTime
    )
    .LogTo(
        message => Debug.WriteLine(message),
        envName == "Development" ? LogLevel.Trace : LogLevel.Error,
        DbContextLoggerOptions.DefaultWithUtcTime
    )
     .EnableDetailedErrors(envName.Equals("Development", StringComparison.Ordinal))
     .EnableSensitiveDataLogging(envName.Equals("Development", StringComparison.Ordinal));
});
#endregion DbContext

//******* Logging *******//
#region Logger
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    options.ResponseHeaders.Add("minimal-openapi-example-api");
    options.MediaTypeOptions.AddText("application/json");
});

// SERILOG
var _logger = new LoggerConfiguration()
  .Enrich.WithProperty("Source", Assembly.GetExecutingAssembly().GetName().Name)
  .Enrich.WithProperty("OSVersion", Environment.OSVersion)
  .Enrich.WithProperty("ServerName", System.Net.Dns.GetHostName())
  .Enrich.WithProperty("UserName", Environment.UserName)
  .Enrich.WithProperty("UserDomainName", Environment.UserDomainName)
  .Enrich.WithProperty("Address", new Shared().GetHostIpAddress())
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();
builder.Logging.AddSerilog(_logger);

if (envName.Equals("Development", StringComparison.Ordinal))
{
    Serilog.Debugging.SelfLog.Enable(Console.Error);
}
#endregion Logger


builder.Services.AddScoped<BatchProcessMessage>();
builder.Services.AddBatchProcessEndPoints();
builder.Services.AddTestBatchEndPoint();


//************** APP **************************************************************//
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "BatchProcess API v1");
        o.RoutePrefix = string.Empty;
    });
}

//app.UseHttpsRedirection();

#region ENDPOINTS
app.MapBatchProcessUseEndPoints(_logger);
app.MapTestBatchUseEndPoint(_logger);
#endregion ENDPOINTS

app.UseHttpLogging();

_logger.Information("--> Environment: {envName}", envName);
_logger.Information("--> Host: {HostIpAddress}", new Shared().GetHostIpAddress());
_logger.Information("--> XML Path: {path}", Path.Combine(AppContext.BaseDirectory, xmlFile));

// if (envName.Equals("Development", StringComparison.Ordinal))
// {
//     _logger.Information(
//         "--> Database Connection: Server={server},{port};Database={database};User={user};Password={password};TrustServerCertificate=True",
//         server, port, database, user, password
//     );
// }

app.Run();
