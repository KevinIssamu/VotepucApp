using Domain.Shared;
using Domain.Shared.AppSuccess;
using Domain.User.Messages.Constants;

namespace Domain.User.Messages;

public record UserCreated() : AppSuccess(ConstantsUserSuccess.UserCreatedSuccessfully, AppSuccessTypeEnum.Success);

public record UserEmailUpdated() : AppSuccess(ConstantsUserSuccess.UserEmailUpdatedSuccessfully, AppSuccessTypeEnum.Success);

public record UserPasswordUpdated() : AppSuccess(ConstantsUserSuccess.UserPasswordUpdatedSuccessfully, AppSuccessTypeEnum.Success);

public record UserNameUpdated() : AppSuccess(ConstantsUserSuccess.UserNameUpdatedSuccessfully, AppSuccessTypeEnum.Success);

public record UserTypeUpdated() : AppSuccess(ConstantsUserSuccess.UserTypeUpdatedSuccessfully, AppSuccessTypeEnum.Success);