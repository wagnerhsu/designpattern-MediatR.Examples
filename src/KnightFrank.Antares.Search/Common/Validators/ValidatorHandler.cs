namespace KnightFrank.Antares.Search.Common.Validators
{
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using MediatR;

    public class ValidatorHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> inner;

        private readonly IValidator<TRequest>[] validators;

        public ValidatorHandler(IRequestHandler<TRequest, TResponse> inner, IValidator<TRequest>[] validators)
        {
            this.inner = inner;
            this.validators = validators;
        }

        public TResponse Handle(TRequest request)
        {
            var context = new ValidationContext(request);

            List<ValidationFailure> failures =
                this.validators.Select(v => v.Validate(context)).SelectMany(result => result.Errors).Where(f => f != null).ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return this.inner.Handle(request);
        }
    }
}