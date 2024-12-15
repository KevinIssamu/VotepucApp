namespace Domain.Shared.AppError;

public enum AppErrorTypeEnum
{
    ValidationFailure = 1, //Usado para validações simples
    BusinessRuleValidationFailure = 2, //Usado para validações mais complexas de regras de negócio
}