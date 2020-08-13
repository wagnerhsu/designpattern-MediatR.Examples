using Common.Command;
using Domain.Order.CommandResponse;
using FluentValidation;

namespace Domain.Order.Command
{
    public class MakeOrderComplaint : ICommand<OrderComplaintInfo>
    {
        public int OrderId { get; set; }

        public string ComplaintMessage { get; set; }
    }

    public class MakeOrderComplaintValidator : AbstractValidator<MakeOrderComplaint>
    {
        public MakeOrderComplaintValidator()
        {
            RuleFor(el => el.ComplaintMessage).NotEmpty();
        }
    }
}