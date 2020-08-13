using MediatR;
using System;

namespace Common.Event
{
    public abstract class DomainEvent : INotification
    {
        protected DomainEvent()
        {
            OccuredOn = DateTime.UtcNow;
        }

        public DateTime OccuredOn { get; private set; }
    }
}