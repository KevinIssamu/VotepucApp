namespace Domain.Shared.AppError;

public record AppError(string Message, AppErrorTypeEnum Type);