using BatchProcess.Api.Models.Entities;
using BatchProcess.Api.Repository;
using Serilog.Core;

namespace BatchProcess.API;

/// <summary>
/// Extension method to map Batch Process API endpoints.
/// </summary>
public static class ApiEndPointExtension
{
    /// <summary>
    /// Configures the IServiceCollection by adding the BatchProcessBapDbRepo and BatchProcessBapNDbRepo services to the dependency injection container.
    /// </summary>
    /// <param name="services">The IServiceCollection to configure.</param>
    /// <returns>The configured IServiceCollection.</returns>
    public static IServiceCollection AddBatchProcessEndPoints(
       this IServiceCollection services)
    {
        services.AddScoped<BatchProcessBapDbRepo>();
        services.AddScoped<BatchProcessBapNDbRepo>();

        return services;
    }

    /// <summary>
    /// Configures the API endpoints for managing Batch Process entities (Bap and BapN).
    /// </summary>
    /// <param name="app">The WebApplication instance to which the endpoints will be added.</param>
    /// <param name="_logger">The Logger instance used for logging errors and other information.</param>
    /// <returns>The WebApplication instance with the configured endpoints.</returns>
    public static WebApplication MapBatchProcessUseEndPoints(
       this WebApplication app, Logger _logger
    )
    {
        //******** BAP *********//
        #region BAP
        app.MapGet(
            "/api/bap",
            async (BatchProcessBapDbRepo _process) =>
            {
                try
                {
                    var bap = await _process.FilterAsNoTrackingAsync();
                    var bapList = bap.ToList();
                    return Results.Ok(bapList);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while retrieving BAP list.");
                    return Results.Problem(ex.Message);
                }
            })
                .Produces<BapDto>(200, "application/json", "application/xml")
                .Produces(401)
                .Produces(404)
                .Produces(429)
                .WithDescription("Get list of Bap")
                .WithTags("Bap")
                .WithName("Get Bap")
                .WithOpenApi();

        app.MapGet(
            "/api/bap/{id}",
            async (BatchProcessBapDbRepo _process, string id) =>
            {
                try
                {
                    if (id == null)
                    {
                        return Results.BadRequest("Id is required");
                    }

                    var bap = await _process.FilterAsNoTrackingAsync(b => b.Bap_Id.ToString() == id);

                    if (!bap.Any())
                    {
                        return Results.NotFound();
                    }

                    var bapFound = bap.FirstOrDefault();

                    return Results.Ok(bapFound);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while retrieving BAP by ID.");
                    return Results.Problem(ex.Message);
                }
            })
                .Produces<BapDto>(200, "application/json", "application/xml")
                .Produces(401)
                .Produces(404)
                .Produces(429)
                .WithDescription("Get Bap by id")
                .WithTags("Bap")
                .WithName("Get Bap id")
                .WithOpenApi();

        app.MapPost(
            "/api/bap",
            async (
                BatchProcessBapDbRepo _process,
                HttpContext context,
                BapDto bapRequest
            ) =>
            {
                try
                {
                    if (bapRequest == null)
                    {
                        return Results.BadRequest("Bap is required");
                    }

                    var bap = new BapDto
                    {
                        Bap_Id = Guid.NewGuid(),
                        Bap_Code = bapRequest.Bap_Code,
                        Bap_State = bapRequest.Bap_State,
                        Bap_Started_DateTime = bapRequest.Bap_Started_DateTime,
                        Bap_Cancelled_DateTime = bapRequest.Bap_Cancelled_DateTime,
                        Bap_Finished_DateTime = bapRequest.Bap_Finished_DateTime,
                        Bap_Failed_DateTime = bapRequest.Bap_Failed_DateTime,
                        Bap_Session_Id = Guid.NewGuid(),
                        CreatedBy = $"{context.Request.Host.Host}.authors"
                    };

                    var bapResult = await _process.CreateAsync(bap);

                    return Results.Ok(bapResult);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while creating BAP.");
                    return Results.Problem(ex.Message);
                }
            })
                .Accepts<BapDto>("application/json", "application/xml")
                .Produces<BapDto>(200, "application/json", "application/xml")
                .Produces(400)
                .Produces(401)
                .Produces(429)
                .WithDescription("Create a new Bap")
                .WithTags("Bap")
                .WithName("Create new Bap")
                .WithOpenApi();

        app.MapMethods(
            "/api/bap/{id}",
            new[] { HttpMethod.Patch.Method },
            async (
                BatchProcessBapDbRepo _process,
                Guid id,
                BapDto bapRequest
            ) =>
            {
                try
                {
                    var existBap = await _process.FilterAsNoTrackingAsync(
                        a => a.Bap_Id == id
                    );
                    if (!existBap.Any())
                    {
                        return Results.NotFound();
                    }

                    var bapToUpdate = existBap.FirstOrDefault();
                    bapRequest.Bap_Id = id!;
                    bapRequest.CreatedBy = bapToUpdate!.CreatedBy;
                    bapRequest.CreatedDate = bapToUpdate!.CreatedDate;

                    await _process.UpdateAsync(a => a.Bap_Id == id, bapRequest);

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while updating BAP.");
                    return Results.Problem(ex.Message);
                }
            })
                .Accepts<BapDto>("application/json", "application/xml")
                .Produces(204)
                .Produces(400)
                .Produces(401)
                .Produces(404)
                .Produces(429)
                .WithDescription("Update Bap")
                .WithTags("Bap")
                .WithName("Update Bap")
                .WithOpenApi();

        app.MapDelete(
            "/api/bap/{id}",
            async (BatchProcessBapDbRepo _process, string id) =>
            {
                try
                {
                    if (id == null)
                    {
                        return Results.BadRequest("Id is required");
                    }

                    var bap = await _process.FilterAsync(
                        b => b.Bap_Id.ToString() == id
                    );

                    if (!bap.Any())
                    {
                        return Results.NotFound();
                    }

                    await _process.DeleteAsync(bap.FirstOrDefault()!);

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while deleting BAP.");
                    return Results.Problem(ex.Message);
                }
            })
                .Produces(204)
                .Produces(401)
                .Produces(429)
                .WithDescription("Delete Bap")
                .WithTags("Bap")
                .WithName("Delete Bap")
                .WithOpenApi();
        #endregion BAP
        //******** BAP ********//


        //******** BAPN ********//
        #region BAPN
        app.MapGet(
            "/api/bapn",
            async (BatchProcessBapNDbRepo _process) =>
            {
                try
                {
                    var bapn = await _process.FilterAsNoTrackingAsync();
                    var bapnList = bapn.ToList();
                    return Results.Ok(bapnList);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while retrieving BAPN list.");
                    return Results.Problem(ex.Message);
                }
            })
                .Produces<BapnDto>(200, "application/json", "application/xml")
                .Produces(401)
                .Produces(404)
                .Produces(429)
                .WithDescription("Get list of BapN")
                .WithTags("BapN")
                .WithName("Get BapN")
                .WithOpenApi();

        app.MapGet(
            "/api/bapn/{id}",
            async (BatchProcessBapNDbRepo _process, string id) =>
            {
                try
                {
                    if (id == null)
                    {
                        return Results.BadRequest("Id is required");
                    }

                    var bapn = await _process.FilterAsNoTrackingAsync(
                        b => b.BapN_Id.ToString() == id
                    );

                    if (!bapn.Any())
                    {
                        return Results.NotFound();
                    }

                    var bapnFound = bapn.FirstOrDefault();

                    return Results.Ok(bapnFound);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while retrieving BAPN by ID.");
                    return Results.Problem(ex.Message);
                }
            })
                .Produces<BapnDto>(200, "application/json", "application/xml")
                .Produces(401)
                .Produces(404)
                .Produces(429)
                .WithDescription("Get BapN by id")
                .WithTags("BapN")
                .WithName("Get BapN id")
                .WithOpenApi();

        app.MapPost(
            "/api/bapn",
            async (
                BatchProcessBapNDbRepo _process,
                HttpContext context,
                BapnDto bapnRequest
            ) =>
            {
                try
                {
                    if (bapnRequest == null)
                    {
                        return Results.BadRequest("BapN is required");
                    }

                    var bapn = new BapnDto
                    {
                        BapN_Id = Guid.NewGuid(),
                        BapN_BapId = bapnRequest.BapN_BapId,
                        BapN_AA = bapnRequest.BapN_AA,
                        BapN_kind = bapnRequest.BapN_kind,
                        BapN_Data = bapnRequest.BapN_Data,
                        BapN_DateTime = bapnRequest.BapN_DateTime,
                        CreatedBy = $"{context.Request.Host.Host}.authors"
                    };

                    var bapnResult = await _process.CreateAsync(bapn);

                    return Results.Ok(bapnResult);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while creating BAPN.");
                    return Results.Problem(ex.Message);
                }
            })
                .Accepts<BapnDto>("application/json", "application/xml")
                .Produces<BapnDto>(200, "application/json", "application/xml")
                .Produces(400)
                .Produces(401)
                .Produces(429)
                .WithDescription("Create a new BapN")
                .WithTags("BapN")
                .WithName("Create new BapN")
                .WithOpenApi();

        app.MapMethods(
            "/api/bapn/{id}",
            new[] { HttpMethod.Patch.Method },
            async (
                BatchProcessBapNDbRepo _process,
                Guid id,
                BapnDto bapnRequest
            ) =>
            {
                try
                {
                    var existBapn = await _process.FilterAsNoTrackingAsync(
                        a => a.BapN_Id == id
                    );
                    if (!existBapn.Any())
                    {
                        return Results.NotFound();
                    }

                    var bapnToUpdate = existBapn.FirstOrDefault();
                    bapnRequest.BapN_Id = id!;
                    bapnRequest.CreatedBy = bapnToUpdate!.CreatedBy;
                    bapnRequest.CreatedDate = bapnToUpdate!.CreatedDate;

                    await _process.UpdateAsync(a => a.BapN_Id == id, bapnRequest);

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while updating BAPN.");
                    return Results.Problem(ex.Message);
                }
            })
                .Accepts<BapnDto>("application/json", "application/xml")
                .Produces(204)
                .Produces(400)
                .Produces(401)
                .Produces(404)
                .Produces(429)
                .WithDescription("Update BapN")
                .WithTags("BapN")
                .WithName("Update BapN")
                .WithOpenApi();

        app.MapDelete(
            "/api/bapn/{id}",
            async (BatchProcessBapNDbRepo _process, string id) =>
            {
                try
                {
                    if (id == null)
                    {
                        return Results.BadRequest("Id is required");
                    }

                    var bapn = await _process.FilterAsync(
                        b => b.BapN_Id.ToString() == id
                    );

                    if (!bapn.Any())
                    {
                        return Results.NotFound();
                    }

                    await _process.DeleteAsync(bapn.FirstOrDefault()!);

                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while deleting BAPN.");
                    return Results.Problem(ex.Message);
                }
            })
                .Produces(204)
                .Produces(401)
                .Produces(429)
                .WithDescription("Delete BapN")
                .WithTags("BapN")
                .WithName("Delete BapN")
                .WithOpenApi();
        #endregion BAPN
        //******** BAPN ********//

        return app;
    }
}
