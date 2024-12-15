namespace Domain.Shared.AppError.GenericErrors;

public record GenericNullError(string PropName) : AppError($"{PropName} cannot be null", AppErrorTypeEnum.ValidationFailure);