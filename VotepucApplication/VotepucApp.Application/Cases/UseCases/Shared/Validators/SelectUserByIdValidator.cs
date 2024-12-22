using FluentValidation;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;

namespace VotepucApp.Application.Cases.UseCases.Shared.Validators;

public class SelectUserByIdValidator<T> : AbstractValidator<SelectUserByIdRequest<T>>
{
    public SelectUserByIdValidator()
    {
        RuleFor(i => i.Id).NotEmpty();
    }
}