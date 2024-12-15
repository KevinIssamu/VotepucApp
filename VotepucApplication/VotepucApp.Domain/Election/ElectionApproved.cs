using Domain.Election.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;

namespace Domain.Election;

public class ElectionApproved : Election
{
    public void Start()
    {
        Progress = ElectionProgress.Active;
    }
    
    public void Finish()
    {
        Progress = ElectionProgress.Finished;
    }
}