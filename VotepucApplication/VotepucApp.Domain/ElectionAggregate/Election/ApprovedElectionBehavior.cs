using Domain.ElectionAggregate.Election.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;

namespace Domain.ElectionAggregate.Election;

public class ApprovedElectionBehavior(Election election)
{
    public OneOf<AppSuccess, AppError> Start()
    {
        if (election.Progress == ElectionProgressEnum.Finish)
            return new AppError("Finished elections cannot be started.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);
        election.Progress = ElectionProgressEnum.Active;
        return new AppSuccess("Election started successfully.");
    }

    public OneOf<AppSuccess, AppError> Starting()
    {
        if(election.Progress != ElectionProgressEnum.Inactive)
            return new AppError("Only inactive progress to be in starting status.", AppErrorTypeEnum.BusinessRuleValidationFailure);
        election.Progress = ElectionProgressEnum.Starting;
        return new AppSuccess("Election starting successfully.");
    }
    
    public OneOf<AppSuccess, AppError> Finish()
    {
        if (election.Progress != ElectionProgressEnum.Active)
            return new AppError("Only active elections can be concluded.", AppErrorTypeEnum.BusinessRuleValidationFailure);
        election.Progress = ElectionProgressEnum.Finish;
        return new AppSuccess("Election finished successfully.");
    }
}