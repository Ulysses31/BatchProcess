using BatchProcess.Api.Database;
using BatchProcess.Api.Database.Repository;
using BatchProcess.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BatchProcess.Api.Repository;

/// <summary>
/// Represents a repository for BatchProcessBapDto objects.
/// </summary>
public class BatchProcessBapDbRepo : MsSqlDatabaseRepo<BapDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BatchProcessBapDbRepo"/> class.
    /// </summary>
    /// <param name="context">The BatchProcessDbContext.</param>
    public BatchProcessBapDbRepo(BatchProcessDbContext context) : base(context)
    {
    }
}

