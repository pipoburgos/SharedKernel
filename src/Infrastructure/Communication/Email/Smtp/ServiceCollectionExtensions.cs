using HealthChecks.Network.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SharedKernel.Application.Communication.Email;
using SharedKernel.Application.Settings;
using SharedKernel.Infrastructure.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharedKernel.Infrastructure.Communication.Email.Smtp;

/// <summary>  </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>  </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddSmtp(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpSettings = new SmtpSettings();
        configuration.GetSection(nameof(SmtpSettings)).Bind(smtpSettings);

        services
            .AddHealthChecks()
            .AddSmtpHealthCheck(setup =>
            {
                setup.Host = smtpSettings.MailServer;
                if (smtpSettings.MailPort != default)
                {
                    setup.Port = smtpSettings.MailPort;
                    setup.ConnectionType = SmtpConnectionType.PLAIN;
                }
                else
                {
                    setup.ConnectionType = smtpSettings.RequireSsl ? SmtpConnectionType.SSL :
                        smtpSettings.RequireTls ? SmtpConnectionType.TLS : SmtpConnectionType.PLAIN;
                }
                if (!string.IsNullOrWhiteSpace(smtpSettings.User) && !string.IsNullOrWhiteSpace(smtpSettings.Password))
                    setup.LoginWith(smtpSettings.User, smtpSettings.Password);

                setup.AllowInvalidRemoteCertificates = true;
            }, "Smtp", tags: new[] { "Smtp" });


        return services
            .AddOptions()
            .Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)))
            .AddTransient(typeof(IOptionsService<>), typeof(OptionsService<>))
            .AddTransient<IEmailSender, SmtpEmailSender>();
    }
}

internal static class Prueba
{
    /// <summary>
    /// Add a health check for network SMTP connection.
    /// </summary>
    /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
    /// <param name="setup">The action to configure SMTP connection parameters.</param>
    /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'smtp' will be used for the name.</param>
    /// <param name="failureStatus">
    /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
    /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
    /// </param>
    /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
    /// <param name="timeout">An optional <see cref="TimeSpan"/> representing the timeout of the check.</param>
    /// <returns>The specified <paramref name="builder"/>.</returns>
    public static IHealthChecksBuilder AddSmtpHealthCheck2(
        this IHealthChecksBuilder builder,
        Action<SmtpHealthCheckOptions2> setup,
        string name,
        HealthStatus? failureStatus = default,
        IEnumerable<string> tags = default,
        TimeSpan? timeout = default)
    {
        var options = new SmtpHealthCheckOptions2();
        setup?.Invoke(options);

        return builder.Add(new HealthCheckRegistration(
            name,
            _ => new SmtpHealthCheck2(options),
            failureStatus,
            tags,
            timeout));
    }
}

internal class SmtpHealthCheckOptions2 : SmtpConnectionOptions2
{
    internal (bool Login, (string, string) Account) AccountOptions { get; private set; }
    public void LoginWith(string userName, string password)
    {
        Domain.Guards.Guard.ThrowIfNull(userName, true);

        AccountOptions = (Login: true, Account: (userName, password));
    }
}

internal class SmtpHealthCheck2 : IHealthCheck
{
    private readonly SmtpHealthCheckOptions2 _options;

    public SmtpHealthCheck2(SmtpHealthCheckOptions2 options)
    {
        _options = options;
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var smtpConnection = new SmtpConnection2(_options);

            if (!await smtpConnection.ConnectAsync(cancellationToken).ConfigureAwait(false))
                return new HealthCheckResult(context.Registration.FailureStatus,
                    $"Could not connect to smtp server {_options.Host}:{_options.Port} - SSL : {_options.ConnectionType}");

            if (!_options.AccountOptions.Login)
                return HealthCheckResult.Healthy();

            var (user, password) = _options.AccountOptions.Account;

            var result = await smtpConnection.AuthenticateAsync(user, password, cancellationToken)
                .ConfigureAwait(false);

            return !result
                ? new HealthCheckResult(context.Registration.FailureStatus,
                    $"Error login to smtp server {_options.Host}:{_options.Port} with configured credentials")
                : HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
        }
    }
}
internal class SmtpConnectionOptions2
{
    public string Host { get; set; } = null!;

    public int Port { get; set; }

    public bool AllowInvalidRemoteCertificates { get; set; }

    public SmtpConnectionType ConnectionType = SmtpConnectionType.AUTO;
}

internal class SmtpConnection2 : MailConnection
{
    private const string ActionOk = "250";
    private const string AuthenticationSuccess = "235";
    private const string ServiceReady = "220";
    private readonly SmtpConnectionOptions2 _options;
    private SmtpConnectionType _connectionType;

    public SmtpConnectionType ConnectionType
    {
        get => _connectionType;

        private set
        {
            _connectionType = value;
            UseSSL = ConnectionType == SmtpConnectionType.SSL;
        }
    }

    public SmtpConnection2(SmtpConnectionOptions2 options)
        : base(options.Host, options.Port, false, options.AllowInvalidRemoteCertificates)
    {
        _options = options;
        ConnectionType = options.ConnectionType;
        ComputeDefaultValues();
    }

    public new async Task<bool> ConnectAsync(CancellationToken cancellationToken = default)
    {
        await base.ConnectAsync(cancellationToken).ConfigureAwait(false);
        return await ExecuteCommandAsync(
            SmtpCommands.EHLO(Host),
            result => FromBase64(result, ActionOk),
            cancellationToken).ConfigureAwait(false);
    }

    public async Task<bool> AuthenticateAsync(string userName, string password, CancellationToken cancellationToken = default)
    {
        if (ShouldUpgradeConnection)
            await UpgradeToSecureConnectionAsync(cancellationToken).ConfigureAwait(false);

        await ExecuteCommandAsync(SmtpCommands.EHLO(Host),
            result => FromBase64(result, ActionOk), cancellationToken).ConfigureAwait(false);

        await ExecuteCommandAsync(SmtpCommands.AUTHLOGIN(),
            result => FromBase64(result, ActionOk), cancellationToken).ConfigureAwait(false);

        await ExecuteCommandAsync($"{ToBase64(userName)}\r\n",
            result => FromBase64(result, ActionOk), cancellationToken).ConfigureAwait(false);

        password = password?.Length > 0 ? ToBase64(password) : string.Empty;

        return await ExecuteCommandAsync($"{password}\r\n",
                result => FromBase64(result, AuthenticationSuccess), cancellationToken)
            .ConfigureAwait(false);
    }

    private bool ShouldUpgradeConnection => !UseSSL && _connectionType != SmtpConnectionType.PLAIN;

    private void ComputeDefaultValues()
    {
        switch (_options.ConnectionType)
        {
            case SmtpConnectionType.AUTO when Port == 465:
                ConnectionType = SmtpConnectionType.SSL;
                break;
            case SmtpConnectionType.AUTO when Port == 587:
                ConnectionType = SmtpConnectionType.TLS;
                break;
            case SmtpConnectionType.AUTO when Port == 25:
                ConnectionType = SmtpConnectionType.PLAIN;
                break;
        }

        if (ConnectionType == SmtpConnectionType.AUTO)
        {
            throw new Exception($"Port {Port} is not a valid smtp port when using automatic configuration");
        }
    }

    private async Task UpgradeToSecureConnectionAsync(CancellationToken cancellationToken)
    {
        var upgradeResult = await ExecuteCommandAsync(SmtpCommands.STARTTLS(),
                result => FromBase64(result, ServiceReady), cancellationToken)
            .ConfigureAwait(false);

        if (!upgradeResult)
            throw new Exception("Could not upgrade SMTP non SSL connection using STARTTLS handshake");

        UseSSL = true;
        _stream = await GetStreamAsync(cancellationToken).ConfigureAwait(false);
    }

    private string ToBase64(string text)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
    }

    private bool FromBase64(byte[] result, string contains)
    {
        var text = Encoding.UTF8.GetString(result);
        return text.StartsWith(contains);
    }
}
