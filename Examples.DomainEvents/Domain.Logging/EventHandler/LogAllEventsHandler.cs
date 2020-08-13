using Common.Event;
using MediatR;
using NLog;

namespace Domain.Logging.EventHandler
{
    public class LogAllEventsHandler : INotificationHandler<DomainEvent>
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public void Handle(DomainEvent @event)
        {
            _logger.Info($"Event {@event.GetType().Name} occured on {@event.OccuredOn}");
        }
    }
}