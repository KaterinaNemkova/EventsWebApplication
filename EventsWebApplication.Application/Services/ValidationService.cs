using FluentValidation;
using FluentValidation.Results;

public class ValidationService
{
    private readonly IEnumerable<IValidator> _validators;

    public ValidationService(IEnumerable<IValidator> validators)
    {
        _validators = validators;
    }

    public async Task ValidateAsync<T>(T instance)
    {
        var validators = _validators.OfType<IValidator<T>>().ToList();

        if (!validators.Any())
        {
            Console.WriteLine($"No validators found for type: {typeof(T).Name}"); // Для отладки
            return; // Здесь вы возвращаете, если валидаторов не найдено
        }

        var validationFailures = new List<ValidationFailure>();

        foreach (var validator in validators)
        {
            var validationResult = await validator.ValidateAsync(instance);
            if (!validationResult.IsValid)
            {
                validationFailures.AddRange(validationResult.Errors);
            }
        }

        if (validationFailures.Any())
        {
            throw new ValidationException(validationFailures);
        }
    }
}
