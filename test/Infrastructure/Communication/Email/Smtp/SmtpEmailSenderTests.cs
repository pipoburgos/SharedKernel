using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Communication.Email;
using SharedKernel.Application.Settings;
using SharedKernel.Infrastructure.Communication.Email;
using SharedKernel.Infrastructure.Communication.Email.Smtp;
using SharedKernel.Infrastructure.Exceptions;
using SharedKernel.Testing.Infrastructure;
using System.Globalization;
using Xunit;

namespace SharedKernel.Integration.Tests.Communication.Email.Smtp
{
    [Collection("DockerHook")]
    public class SmtpEmailSenderTests : InfrastructureTestCase<FakeStartup>
    {
        protected override string GetJsonFile()
        {
            return "Communication/Email/Smtp/appsettings.smtp.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services.AddSmtp(Configuration);
        }

        [Fact]
        public async Task SendEmailOk()
        {
            var sender = new SmtpEmailSender(GetRequiredServiceOnNewScope<IOptionsService<SmtpSettings>>());

            var result = async () => await sender.SendEmailAsync(EmailTestFactory.Create(), CancellationToken.None);

            await result.Should().NotThrowAsync();
        }

        [Fact]
        public async Task SendEmailWithAttachmentOk()
        {
            var sender = new SmtpEmailSender(GetRequiredServiceOnNewScope<IOptionsService<SmtpSettings>>());

            var bytes = await GetPhotoBinary();

            var attachment = new MailAttachment("Adjunto.jpg", bytes);

            var result = async () => await sender.SendEmailAsync(EmailTestFactory.Create(attachment), CancellationToken.None);

            await result.Should().NotThrowAsync();
        }

        [Fact]
        public async Task SendEmailWithAttachmentNotFilenameExtensionKo()
        {
            var sender = new SmtpEmailSender(GetRequiredServiceOnNewScope<IOptionsService<SmtpSettings>>());

            var bytes = await GetPhotoBinary();

            var attachment = new MailAttachment("Adjunto", bytes);

            var result = async () => await sender.SendEmailAsync(EmailTestFactory.Create(attachment), CancellationToken.None);

            await result.Should().ThrowAsync<EmailException>().WithMessage(ExceptionCodes.EMAIL_ATTACH_EXT);
        }

        private static Task<FileStream> GetPhotoBinary()
        {
            const string path = "Communication/Email/Photo.jpg";
            return Task.FromResult(new FileStream(path, FileMode.Open, FileAccess.Read));
        }

        [Fact]
        public async Task SendEmailEmptyPasswordTaskKo()
        {
            var smtp = GetRequiredServiceOnNewScope<IOptionsService<SmtpSettings>>();
            smtp.Value.Password = null;
            var sender = new SmtpEmailSender(smtp);

            var bytes = await GetPhotoBinary();

            var attachment = new MailAttachment("Adjunto", bytes);

            var result = async () => await sender.SendEmailAsync(EmailTestFactory.Create(attachment), CancellationToken.None);

            await result.Should().ThrowAsync<EmailException>().WithMessage(ExceptionCodes.SMT_PASS_EMPTY);
        }

        [Fact]
        public async Task SendEmailEmptyPasswordTaskSpanishKo()
        {
            var defaultCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");

            var smtp = GetRequiredServiceOnNewScope<IOptionsService<SmtpSettings>>();
            smtp.Value.Password = null;
            var sender = new SmtpEmailSender(smtp);

            var bytes = await GetPhotoBinary();

            var attachment = new MailAttachment("Adjunto", bytes);

            var result = async () => await sender.SendEmailAsync(EmailTestFactory.Create(attachment), CancellationToken.None);

            await result.Should().ThrowAsync<EmailException>().WithMessage(ExceptionCodes.SMT_PASS_EMPTY);

            Thread.CurrentThread.CurrentUICulture = defaultCulture;
        }
    }
}