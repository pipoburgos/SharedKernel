﻿using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Application.Communication.Email;
using SharedKernel.Application.Settings;
using SharedKernel.Infrastructure.Communication.Email;
using SharedKernel.Infrastructure.Communication.Email.Smtp;
using SharedKernel.Infrastructure.Exceptions;
using SharedKernel.Infrastructure.Settings;
using SharedKernel.Integration.Tests.Shared;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SharedKernel.Integration.Tests.Communication
{
    public class EmailSenderTests : InfrastructureTestCase
    {
        protected override string GetJsonFile()
        {
            return "appsettings.smtp.json";
        }

        protected override IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IOptionsService<>), typeof(OptionsService<>));
            services.Configure<SmtpSettings>(Configuration.GetSection(nameof(SmtpSettings)));
            return services;
        }

        [Fact]
        public async Task SendEmailOk()
        {
            var sender = new SmtpEmailSender(GetService<IOptionsService<SmtpSettings>>());

            await sender.SendEmailAsync("sharedkerneltests@gmail.com", "Testing", "Prueba testing");

            Assert.True(true);
        }

        [Fact]
        public async Task SendEmailWithAttachmentOk()
        {
            var sender = new SmtpEmailSender(GetService<IOptionsService<SmtpSettings>>());

            var path = $"{Directory.GetCurrentDirectory()}\\Photo.jpg";
            var bytes = File.ReadAllBytes(path);

            var attachment = new EmailAttachment("Adjunto.jpg", bytes);

            await sender.SendEmailAsync("sharedkerneltests@gmail.com", "Testing con adjunto",
                "Prueba testing con adjunto", attachment);

            Assert.True(true);
        }

        [Fact]
        public async Task SendEmailWithAttachmentNotFilenameExtensionKo()
        {
            var sender = new SmtpEmailSender(GetService<IOptionsService<SmtpSettings>>());

            var path = $"{Directory.GetCurrentDirectory()}\\Photo.jpg";
            var bytes = File.ReadAllBytes(path);

            var attachment = new EmailAttachment("Adjunto", bytes);

            Task Action()
            {
                return sender.SendEmailAsync("sharedkerneltests@gmail.com", "Testing con adjunto",
                    "Prueba testing con adjunto", attachment);
            }

            var exception = await Assert.ThrowsAsync<EmailException>(Action);
            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal(ExceptionCodes.EMAIL_ATTACH_EXT, exception.Message);
        }

        [Fact]
        public async Task SendEmailEmptyPasswordTaskKo()
        {
            var smtp = GetService<IOptionsService<SmtpSettings>>();
            smtp.Value.Password = null;
            var sender = new SmtpEmailSender(smtp);

            var path = $"{Directory.GetCurrentDirectory()}\\Photo.jpg";
            var bytes = File.ReadAllBytes(path);

            var attachment = new EmailAttachment("Adjunto", bytes);

            Task Action()
            {
                return sender.SendEmailAsync("sharedkerneltests@gmail.com", "Testing con adjunto",
                    "Prueba testing con adjunto", attachment);
            }

            var exception = await Assert.ThrowsAsync<EmailException>(Action);
            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal(ExceptionCodes.SMT_PASS_EMPTY, exception.Message);
        }

        [Fact]
        public async Task SendEmailEmptyPasswordTaskSpanishKo()
        {
            var defaultCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");

            var smtp = GetService<IOptionsService<SmtpSettings>>();
            smtp.Value.Password = null;
            var sender = new SmtpEmailSender(smtp);

            var path = $"{Directory.GetCurrentDirectory()}\\Photo.jpg";
            var bytes = File.ReadAllBytes(path);

            var attachment = new EmailAttachment("Adjunto", bytes);

            Task Action()
            {
                return sender.SendEmailAsync("sharedkerneltests@gmail.com", "Testing con adjunto",
                    "Prueba testing con adjunto", attachment);
            }

            var exception = await Assert.ThrowsAsync<EmailException>(Action);
            //The thrown exception can be used for even more detailed assertions.
            Assert.Equal(ExceptionCodes.SMT_PASS_EMPTY, exception.Message);

            Thread.CurrentThread.CurrentUICulture = defaultCulture;
        }
    }
}