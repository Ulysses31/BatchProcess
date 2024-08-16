using BatchProcess.Api.Database;
using BatchProcess.Api.Database.Repository;
using BatchProcess.Api.Models.Entities;
using BatchProcess.API.Services;

namespace BatchProcess.Api.Repository;

/// <summary>
/// Represents a repository for TestBatchProcessDbRepo objects.
/// </summary>
public class TestBatchProcessDbRepo : MsSqlDatabaseRepo<PostDto>
{
    private readonly BatchProcessDbContext _context;
    private readonly BatchProcessMessage _batchProcessMessage;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestBatchProcessDbRepo"/> class.
    /// </summary>
    /// <param name="context">The BatchProcessDbContext.</param>
    /// <param name="batchProcessMessage">The BatchProcessMessage</param>
    public TestBatchProcessDbRepo(
        BatchProcessDbContext context,
        BatchProcessMessage batchProcessMessage
    ) : base(context)
    {
        _context = context;
        _batchProcessMessage = batchProcessMessage;
    }

    /// <summary>
    /// Creates a bulk of PostDto entities and adds them to the context.
    /// </summary>
    /// <param name="countRecords">The number of PostDto records to create.</param>
    /// <returns>An array of created PostDto entities.</returns>
    /// <exception cref="ArgumentException">Thrown when the number of records is zero.</exception>
    /// <exception cref="Exception">Thrown when an error occurs during the creation or saving of entities.</exception>
    public PostDto[] CreateBulk(int countRecords)
    {
        PostDto[] postDtos = new PostDto[countRecords];

        try
        {
            if (countRecords == 0)
            {
                throw new ArgumentException("Number of records is required.");
            }

            for (int i = 1; i <= countRecords; i++)
            {
                var entity = new PostDto
                {
                    PostId = i,
                    UserId = i,
                    Title = $"Title {i}",
                    Body = $"Body {i}"
                };

                postDtos.SetValue(entity, i);

                _context.ChangeTracker.DetectChanges();
                Console.WriteLine("--> {track}", _context.ChangeTracker.DebugView.LongView);
            }

            _context.Set<PostDto>().AddRange(postDtos);

            _context.SaveChanges();

            return (PostDto[])Convert.ChangeType(postDtos, typeof(PostDto));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex.InnerException);
        }
    }

    /// <summary>
    /// Asynchronously creates a bulk of PostDto entities and adds them to the context.
    /// </summary>
    /// <param name="countRecords">The number of PostDto records to create.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains an array of created PostDto entities.</returns>
    /// <exception cref="ArgumentException">Thrown when the number of records is zero.</exception>
    /// <exception cref="Exception">Thrown when an error occurs during the creation or saving of entities.</exception>
    public async Task CreateBulkAsync(int countRecords)
    {
        // using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            if (countRecords == 0)
            {
                throw new ArgumentException("Number of records is required.");
            }

            IList<PostDto> postDtos = new List<PostDto>();

            _batchProcessMessage.StartProcessMessage(
                "CEM_CREATE_AFN",
                "Δημιουργία διαδικασίας καταχώρησης"
            );

            _batchProcessMessage.InProgressProcessMessage();

            _batchProcessMessage.TotalCountProcessMessage(countRecords);

            Console.WriteLine("--> CountRecords: " + countRecords);

            for (int i = 0; i < countRecords; i++)
            {
                _batchProcessMessage.ProgressProcessMessage(i + 1);

                var entity = new PostDto
                {
                    UserId = i + 1,
                    Title = $"Title {i}",
                    Body = $"Body {i}",
                    CreatedBy = "Tester"
                };

                postDtos.Add(entity);
            }

            Task t1 = _context.Posts!.AddRangeAsync(postDtos);
            Task t2 = _context.SaveChangesAsync();
            await Task.WhenAll(t1, t2);

            // Commit the transaction if everything is successful
            // await transaction.CommitAsync();

            _batchProcessMessage.SuccessProcessMessage();

            // return await Task.FromResult(postDtos);
        }
        catch (Exception ex)
        {
            // Rollback the transaction in case of an error
            // await transaction.RollbackAsync();

            _batchProcessMessage.FailureProcessMessage(ex.Message ?? ex.InnerException!.Message);
            Console.WriteLine(ex.Message ?? ex.InnerException!.Message, ex.InnerException);
            throw new Exception(ex.Message ?? ex.InnerException!.Message, ex.InnerException);
        }
    }
}

