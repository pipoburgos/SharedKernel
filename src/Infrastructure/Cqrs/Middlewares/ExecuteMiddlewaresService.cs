using SharedKernel.Application.Validator;
using SharedKernel.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Cqrs.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class ExecuteMiddlewaresService
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public ExecuteMiddlewaresService(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        public void Execute(IRequest request)
        {
            var validator = (IEntityValidator<IRequest>)
                _serviceProvider.GetService(typeof(IEntityValidator<IRequest>));

            if(validator == null)
                throw new ArgumentNullException(nameof(IEntityValidator<IRequest>));

            validator.Validate(request);

            // Todo run middlewares
            //var middlewares = _serviceProvider.GetServices<IMiddleware<IRequest>>();

            //foreach (var middleware in middlewares)
            //{
            //    await middleware.Handle(request, cancellationToken,)
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task ExecuteAsync(IRequest request, CancellationToken cancellationToken)
        {
            var validator = (IEntityValidator<IRequest>)
                _serviceProvider.GetService(typeof(IEntityValidator<IRequest>));

            if (validator == null)
                throw new ArgumentNullException(nameof(IEntityValidator<IRequest>));

            return validator.ValidateAsync(request, cancellationToken);

            // Todo run middlewares
            //var middlewares = _serviceProvider.GetServices<IMiddleware<IRequest>>();

            //foreach (var middleware in middlewares)
            //{
            //    await middleware.Handle(request, cancellationToken,)
            //}
        }
    }
}
