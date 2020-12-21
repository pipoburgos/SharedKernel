using System;
using System.Linq;

namespace SharedKernel.Infrastructure.Events
{
    public class DomainEventSubscriberInformation
    {
        private readonly Type _subscriberClass;
        public Type SubscribedEvent { get; private set; }

        public DomainEventSubscriberInformation(Type subscriberClass, Type subscribedEvent)
        {
            SubscribedEvent = subscribedEvent;
            _subscriberClass = subscriberClass;
        }

        private string ClassName
        {
            get
            {
                var nameParts = _subscriberClass.FullName?.Split('.');
                return nameParts?.Last();
            }
        }

        public string SubscriberName()
        {
            return ToSnake(ClassName);
        }

        private static string ToSnake(string text)
        {
            return string.Concat(text.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()))
                .ToLowerInvariant();
        }
    }
}