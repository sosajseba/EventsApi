using EventsApi.Dtos;
using FluentValidation;

namespace EventsApi.Validators;

public class EventValidator : AbstractValidator<PostEvent>
{
    public EventValidator()
    {
        RuleFor(x => x.Price).NotNull().GreaterThan(0);
        RuleFor(x => x.Date).NotNull().GreaterThan(DateTime.UtcNow);
        RuleFor(x => x.Type).NotNull().NotEmpty().MinimumLength(5).MaximumLength(100);
        RuleFor(x => x.Name).NotNull().NotEmpty().MinimumLength(5).MaximumLength(100);
        RuleFor(x => x.Title).NotNull().NotEmpty().MinimumLength(5).MaximumLength(500);
        RuleFor(x => x.Location).NotNull().NotEmpty().MinimumLength(5).MaximumLength(500);
        RuleFor(x => x.Description).NotNull().NotEmpty().MinimumLength(5).MaximumLength(500);
    }
}