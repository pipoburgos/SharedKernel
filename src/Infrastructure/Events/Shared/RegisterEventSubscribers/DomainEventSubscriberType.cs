using System;
using System.Linq;

namespace SharedKernel.Infrastructure.Events.Shared.RegisterEventSubscribers
{
    /// <summary>
    /// 
    /// </summary>
    public class DomainEventSubscriberType : IDomainEventSubscriberType
    {
        private readonly Type _subscriberClass;

        /// <summary>
        /// 
        /// </summary>
        public Type SubscribedEvent { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriberClass"></param>
        /// <param name="subscribedEvent"></param>
        public DomainEventSubscriberType(Type subscriberClass, Type subscribedEvent)
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

        /// <summary> Gets the type of the subscriber class. </summary>
        public Type GetSubscriber()
        {
            return _subscriberClass;
        }

        /// <summary> Gets the name of the subscriber class. </summary>
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