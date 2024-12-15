using Domain.Shared.AppError.Constants;

namespace Domain.Shared.AppError;

public record NameExceededMaxLength() : AppError($"The maximum name length must be {ConstantsMaxLength.PersonNameMaxLength} characters", 
    AppErrorTypeEnum.ValidationFailure);