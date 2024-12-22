using Domain.Shared.AppSuccess;
using Domain.UserAggregate.User.Messages.Constants;

namespace Domain.UserAggregate.User.Messages;

public record UserCreated() : AppSuccess(ConstantsUserSuccess.UserCreatedSuccessfully);

public record UserEmailUpdated() : AppSuccess(ConstantsUserSuccess.UserEmailUpdatedSuccessfully);

public record UserPasswordUpdated() : AppSuccess(ConstantsUserSuccess.UserPasswordUpdatedSuccessfully);

public record UserNameUpdated() : AppSuccess(ConstantsUserSuccess.UserNameUpdatedSuccessfully);

public record UserTypeUpdated() : AppSuccess(ConstantsUserSuccess.UserTypeUpdatedSuccessfully);