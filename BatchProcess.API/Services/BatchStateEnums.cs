namespace BatchProcess.API.Services;

/// <summary>
/// Enumeration representing the various states of a batch process.
/// </summary>
public enum BatchStateEnums
{
    /// <summary>
    /// The initial state of the batch process.
    /// </summary>
    /// <remarks>
    /// Greek Translation: Αρχική
    /// </remarks>
    StartProcess = 0, // Αρχική

    /// <summary>
    /// The batch process is currently ongoing and actively being executed.
    /// </summary>
    /// <remarks>
    /// Greek Translation: Σε εξέλιξη
    /// </remarks>
    InProgressProcess = 1, // Σε εξέλιξη

    /// <summary>
    /// The batch process has been interrupted and is not currently proceeding.
    /// </summary>
    /// <remarks>
    /// Greek Translation: Διακόπηκε
    /// </remarks>
    InterruptProcess = 2, // Διακόπηκε

    /// <summary>
    /// The batch process has failed to complete successfully.
    /// </summary>
    /// <remarks>
    /// Greek Translation: Απέτυχε
    /// </remarks>
    FailureProcess = 3, // Απέτυχε

    /// <summary>
    /// The batch process has completed successfully.
    /// </summary>
    /// <remarks>
    /// Greek Translation: Ολοκληρώθηκε
    /// </remarks>
    EndProcess = 4 // Ολοκληρώθηκε
}
