using Domain.Election.Messages.Constants;
using Domain.Shared.AppSuccess;

namespace Domain.Election.Messages;

public record ElectionTileSuccessfullyUpdated() : AppSuccess(
    ConstantsElectionSuccess.ElectionTitleSuccessfullyUpdated, AppSuccessTypeEnum.Success);

public record ElectionDescriptionSuccessfullyUpdated() : AppSuccess(
    ConstantsElectionSuccess.ElectionDescriptionSuccessfullyUpdated, AppSuccessTypeEnum.Success);    
    
public record ElectionInviteEmailTextSuccessfullyUpdated() : AppSuccess(
    ConstantsElectionSuccess.ElesctionInviteEmailTextSuccessfullyUpdated, AppSuccessTypeEnum.Success);
    
public record ElectionStartDateSuccessfullyUpdated() : AppSuccess(
    ConstantsElectionSuccess.ElectionStartDateSuccessfullyUpdated, AppSuccessTypeEnum.Success);

public record ElectionEndDateSuccessfullyUpdated() : AppSuccess(
    ConstantsElectionSuccess.ElectionEndDateSuccessfullyUpdated, AppSuccessTypeEnum.Success);

public record ElectionApprovedSuccessfully() : AppSuccess(
    ConstantsElectionSuccess.ElectionApprovedSuccessfully, AppSuccessTypeEnum.Success);  

public record ElectionRejectedSuccessfully() : AppSuccess(
    ConstantsElectionSuccess.ElectionRejectedSuccessfully, AppSuccessTypeEnum.Success);


    
    