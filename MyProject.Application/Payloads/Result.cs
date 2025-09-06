namespace MyProject.Application.Payloads;

/// <summary>
/// Represents a standardized result for API operations, including success status, message, code, and data.
/// </summary>
/// <typeparam name="T">The type of the data returned in the result.</typeparam>
public class Result<T>
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    /// A message describing the result of the operation.
    /// </summary>
    public string Message { get; init; }

    /// <summary>
    /// The status code associated with the result (e.g., HTTP status code).
    /// </summary>
    public int Code { get; init; }

    /// <summary>
    /// The data returned by the operation, if any.
    /// </summary>
    public T Data { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    /// <param name="success">Indicates whether the operation was successful.</param>
    /// <param name="code">The status code associated with the result.</param>
    /// <param name="message">A message describing the result.</param>
    /// <param name="data">The data returned by the operation.</param>
    public Result(bool success, int code, string message, T data)
    {
        Success = success;
        Message = message;
        Code = code;
        Data = data;
    }
    
    /// <summary>
    /// Creates a successful result with the specified data, message, and code.
    /// </summary>
    /// <param name="data">The data to return.</param>
    /// <param name="message">An optional message.</param>
    /// <param name="code">An optional status code (default is 200).</param>
    /// <returns>A successful <see cref="Result{T}"/> instance.</returns>
    public static Result<T> Ok(T data, string message = "", int code = 200) => new(true, code, message, data);
    
    /// <summary>
    /// Creates a failed result with the specified message and code.
    /// </summary>
    /// <param name="message">A message describing the failure.</param>
    /// <param name="code">An optional status code (default is 400).</param>
    /// <returns>A failed <see cref="Result{T}"/> instance.</returns>
    public static Result<T> Fail(string message, int code = 400) => new(false, code, message, default!);
}