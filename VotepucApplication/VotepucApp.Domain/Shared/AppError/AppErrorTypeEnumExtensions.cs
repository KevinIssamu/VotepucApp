namespace Domain.Shared.AppError;

public static class AppErrorTypeEnumExtensions
{
    public static int ToHttpStatusCode(this AppErrorTypeEnum errorType)
    {
        return errorType switch
        {
            AppErrorTypeEnum.SystemError => 500,
            AppErrorTypeEnum.BusinessRuleValidationFailure => 400,
            AppErrorTypeEnum.ValidationFailure => 400,
            AppErrorTypeEnum.NotFound => 404,
            _ => 500
        };
    }
}