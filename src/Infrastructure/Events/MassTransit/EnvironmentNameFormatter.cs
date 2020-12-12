using MassTransit.Topology;

namespace SharedKernel.Infrastructure.Events.MassTransit
{
    public class EnvironmentNameFormatter : IEntityNameFormatter
    {
        private readonly IEntityNameFormatter _original;

        public EnvironmentNameFormatter(IEntityNameFormatter original)
        {
            _original = original;
        }

        public string FormatEntityName<T>()
        {
            var a =  $"{_original.FormatEntityName<T>()}";
            return a;
        }
    }
}
