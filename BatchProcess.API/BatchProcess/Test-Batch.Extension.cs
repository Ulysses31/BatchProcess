using BatchProcess.Api.Models.Entities;
using BatchProcess.Api.Repository;
using BatchProcess.API.BatchProcess.RequestModels;
using BatchProcess.API.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog.Core;

namespace BatchProcess.API;

/// <summary>
/// Extension method to map Batch Process API endpoints.
/// </summary>
public static class TestBatchExtension
{
    /// <summary>
    /// Configures the IServiceCollection by adding the TestBatchProcessDbRepo service to the dependency injection container.
    /// </summary>
    /// <param name="services">The IServiceCollection to configure.</param>
    /// <returns>The configured IServiceCollection.</returns>
    public static IServiceCollection AddTestBatchEndPoint(
        this IServiceCollection services)
    {
        services.AddScoped<TestBatchProcessDbRepo>();

        return services;
    }


    /// <summary>
    /// Configures the API endpoints for managing Batch Process entities (Bap and BapN).
    /// </summary>
    /// <param name="app">The WebApplication instance to which the endpoints will be added.</param>
    /// <param name="_logger">The Logger instance used for logging errors and other information.</param>
    /// <returns>The WebApplication instance with the configured endpoints.</returns>
    public static WebApplication MapTestBatchUseEndPoint(
       this WebApplication app,
       Logger _logger
    )
    {
        app.MapPost(
            "/api/test-batch-process",
            async (
                TestBatchProcessDbRepo _process,
                TestBatchRequest countRequest
            ) =>
            {
                try
                {
                    if (countRequest.countRecords == 0)
                    {
                        return Results.BadRequest("Number of records is required.");
                    }

                    await _process.CreateBulkAsync(countRecords: countRequest.countRecords);

                    return Results.Accepted();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while creating batch process.");
                    return Results.Problem(ex.Message);
                }
            })
                .Accepts<TestBatchRequest>("application/json", "application/xml")
                .Produces(202)
                .Produces(400)
                .Produces(401)
                .Produces(429)
                .WithDescription("Create a new Test Batch Process")
                .WithTags("Batch Process Test")
                .WithName("Create new test batch process")
                .WithOpenApi();

        return app;
    }
}
