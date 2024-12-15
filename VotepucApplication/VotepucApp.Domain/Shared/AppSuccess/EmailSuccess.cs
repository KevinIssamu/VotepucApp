using Domain.Shared.AppSuccess.Constants;

namespace Domain.Shared.AppSuccess;

public record EmailIsValid() : AppSuccess(ConstantsSuccessEmail.EmailIsValid, AppSuccessTypeEnum.Success);