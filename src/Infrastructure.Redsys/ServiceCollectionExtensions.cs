using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Security.Cryptography;
using SharedKernel.Application.Serializers;
using SharedKernel.Infrastructure.Redsys.Contracts;
using SharedKernel.Infrastructure.Redsys.Services;
using SharedKernel.Infrastructure.Security.Cryptography;

namespace SharedKernel.Infrastructure.Redsys;

/// <summary> . </summary>
public static class ServiceCollectionExtensions
{
    /// <summary> . </summary>
    public static IServiceCollection AddSharedKernelRedsys<T>(this IServiceCollection services, IConfiguration configuration,
        Action<RedsysOptions>? configureOptions = null) where T : class, IJsonSerializer
    {
        var redsysOptions = new RedsysOptions();
        configuration.GetSection(nameof(RedsysOptions)).Bind(redsysOptions);
        configureOptions?.Invoke(redsysOptions);

        return services
            .AddTransient<IBase64, Base64>()
            .AddTransient<ISha256, Sha256>()
            .AddTransient<ITripleDes, TripleDes>()
            .AddTransient<IJsonSerializer, T>()
            .AddTransient<IMerchantParametersManager, MerchantParametersManager>()
            .AddTransient<ISignatureComparer, SignatureComparer>()
            .AddTransient<ISignatureManager, SignatureManager>()
            .AddTransient<IPaymentRequestService, PaymentRequestService>()
            .AddTransient<IPaymentResponseService, PaymentResponseService>();
    }
}
