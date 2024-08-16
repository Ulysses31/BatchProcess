namespace BatchProcess.API.BatchProcess.RequestModels
{
    /// <summary>
    /// Represents a request model for creating a batch of test records.
    /// </summary>
    public class TestBatchRequest
    {
        /// <summary>
        /// Gets or sets the number of records to be created.
        /// </summary>
        public int countRecords { get; set; }
    }
}
