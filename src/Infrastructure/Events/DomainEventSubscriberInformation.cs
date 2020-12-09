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

        public string ContextName
        {
            get
            {
                var nameParts = _subscriberClass.FullName?.Split('.');
                return nameParts?[1];
            }
        }

        public string ModuleName
        {
            get
            {
                var nameParts = _subscriberClass.FullName?.Split('.');
                return nameParts?[2];
            }
        }

        public string ClassName
        {
            get
            {
                var nameParts = _subscriberClass.FullName?.Split('.');
                return nameParts?.Last();
            }
        }

        public string SubscriberName()
        {
            // return $"{ToSnake(ContextName)}.{ToSnake(ModuleName)}.{ToSnake(ClassName)}";
            return ToSnake(ClassName);
        }

        public static string ToSnake(string text)
        {
            return string.Concat(text.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()))
                .ToLowerInvariant();
        }
    }
}