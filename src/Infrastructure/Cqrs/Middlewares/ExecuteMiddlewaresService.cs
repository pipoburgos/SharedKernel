using SharedKernel.Application.Validator;
using SharedKernel.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Cqrs.Middlewares
{
    public class ExecuteMiddlewaresService
    {
        private readonly IServiceProvider _serviceProvider;

        public ExecuteMiddlewaresService(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Execute(IRequest request)
        {
            var validator = (IEntityValidator<IRequest>)
                _serviceProvider.GetService(typeof(IEntityValidator<IRequest>));

            if(validator == null)
                throw new Exception("Falta registrar la interfaz IEntityValidator<>");

            validator.Validate(request);

            // Todo ejecutar middlewares
            //var middlewares = _serviceProvider.GetServices<IMiddleware<IRequest>>();

            //foreach (var middleware in middlewares)
            //{
            //    await middleware.Handle(request, cancellationToken,)
            //}
        }

        public Task ExecuteAsync(IRequest request, CancellationToken cancellationToken)
        {
            var validator = (IEntityValidator<IRequest>)
                _serviceProvider.GetService(typeof(IEntityValidator<IRequest>));

            return validator.ValidateAsync(request, cancellationToken);

            // Todo ejecutar middlewares
            //var middlewares = _serviceProvider.GetServices<IMiddleware<IRequest>>();

            //foreach (var middleware in middlewares)
            //{
            //    await middleware.Handle(request, cancellationToken,)
            //}
        }
    }
}
