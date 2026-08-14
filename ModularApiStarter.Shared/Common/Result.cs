namespace ModularApiStarter.Shared.Common
{
    public interface IResult<TSelf> where TSelf : IResult<TSelf>
    {
        static abstract TSelf ValidationFailure(List<string> errors);
    }

    public class Result<T> : IResult<Result<T>>
    {
        public bool IsSuccess { get; init; }
        public bool IsFailure => !IsSuccess;
        public T? Data { get; init; }
        public string Message { get; init; } = string.Empty;
        public List<string> Errors { get; init; } = [];
        public ExceptionType ExceptionType { get; set; } = ExceptionType.None;

        public static Result<T> Success(T data, string message = "Success") =>
            new() { IsSuccess = true, Data = data, Message = message };

        public static Result<T> Failure(string message = "Failed", List<string>? errors = null, ExceptionType exceptionType = ExceptionType.None) =>
            new() { IsSuccess = false, Message = message, Errors = errors ?? [], ExceptionType = exceptionType };

        public static Result<T> ValidationFailure(List<string> errors) =>
            Failure("Validation failed", errors, ExceptionType.Validation);
    }

    public class Result : Result<object>, IResult<Result>
    {
        public static Result Success(string message = "Success") =>
            new() { IsSuccess = true, Message = message };

        public new static Result Failure(string message = "Failed", List<string>? errors = null, ExceptionType exceptionType = ExceptionType.None) =>
            new() { IsSuccess = false, Message = message, Errors = errors ?? [], ExceptionType = exceptionType };

        public new static Result ValidationFailure(List<string> errors) =>
            Failure("Validation failed", errors, ExceptionType.Validation);
    }
}