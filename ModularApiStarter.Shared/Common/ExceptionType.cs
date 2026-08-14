namespace ModularApiStarter.Shared.Common
{
    public enum ExceptionType
    {
        None = 0,
        AlreadyExists = 1,
        Forbidden = 2,
        NotFound = 3,
        Unauthorized = 4,
        Validation = 5,
        InternalServerError = 6
    }
}
