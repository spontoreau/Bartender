namespace Cheers.Cqrs
{
    /// <summary>
    /// Define a CQRS error
    /// </summary>
    public interface ICqrsError
    {
        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <value>The code.</value>
        int Code { get; }

        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>The error.</value>
        string Error { get; }
    }
}

