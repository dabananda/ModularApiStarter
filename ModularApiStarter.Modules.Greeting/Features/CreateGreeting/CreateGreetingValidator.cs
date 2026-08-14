using FluentValidation;

namespace ModularApiStarter.Modules.Greeting.Features.CreateGreeting
{
    // Picked up automatically by AddValidators() + ValidationBehavior<,> in the
    // shared pipeline — no manual wiring needed per-handler.
    public class CreateGreetingValidator : AbstractValidator<CreateGreetingCommand>
    {
        public CreateGreetingValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must be 100 characters or fewer.");
        }
    }
}
