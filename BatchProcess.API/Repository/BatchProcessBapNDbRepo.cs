using BatchProcess.Api.Database;
using BatchProcess.Api.Database.Repository;
using BatchProcess.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BatchProcess.Api.Repository;

/// <summary>
/// Represents a repository for BatchProcessBapNDbRepo objects.
/// </summary>
public class BatchProcessBapNDbRepo : MsSqlDatabaseRepo<BapnDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BatchProcessBapNDbRepo"/> class.
    /// </summary>
    /// <param name="context">The BatchProcessDbContext.</param>
    public BatchProcessBapNDbRepo(BatchProcessDbContext context) : base(context)
    {
    }
}
