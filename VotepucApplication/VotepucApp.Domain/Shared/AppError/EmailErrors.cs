using Domain.Shared.AppError.Constants;

namespace Domain.Shared.AppError;


public record UserEmailIsNotValid() : AppError(ConstantsErrorsEmail.UserEmailIsNotValid, AppErrorTypeEnum.ValidationFailure);

public record UserEmailExceededMaxLength() : AppError($"The maximum email length must be {ConstantsMaxLength.PersonEmailMaxLength} characters",
    AppErrorTypeEnum.ValidationFailure);
