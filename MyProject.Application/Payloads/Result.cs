namespace MyProject.Application.Payloads;

public class Result<T>
{
    public bool Success { get; init; }
    public string Message { get; init; }
    public int Code { get; init; }
    public T Data { get; init; }
    
    public Result(bool success, string message, int code, T data)
    {
        Success = success;
        Message = message;
        Code = code;
        Data = data;
    }
    
    public static Result<T> Ok(T data, string message = "", int code = 200) => new(true, message, code, data);
    
    public static Result<T> Fail(string message, int code = 400) => new(false, message, code, default!);
}