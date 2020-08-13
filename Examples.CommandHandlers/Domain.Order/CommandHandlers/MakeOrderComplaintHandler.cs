using Common.Handler;
using Domain.Order.Command;
using Domain.Order.CommandResponse;

namespace Domain.Order.CommandHandlers
{
    public class MakeOrderComplaintHandler : ValidatorHandler<MakeOrderComplaint, OrderComplaintInfo>
    {
        public override OrderComplaintInfo HandleRequest(MakeOrderComplaint request)
        {
            // find compained order
            // save order complaint to database
            // send email to shop
            // return complaint into

            return new OrderComplaintInfo();
        }
    }
}