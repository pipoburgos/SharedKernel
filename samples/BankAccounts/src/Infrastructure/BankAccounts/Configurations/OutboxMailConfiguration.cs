using Newtonsoft.Json;
using SharedKernel.Application.Communication.Email;

namespace BankAccounts.Infrastructure.BankAccounts.Configurations;

/// <summary>  </summary>
public class OutboxMailConfiguration : IEntityTypeConfiguration<OutboxMail>
{
    /// <summary>  </summary>
    public void Configure(EntityTypeBuilder<OutboxMail> builder)
    {
        builder
            .Property(x => x.To)
            .HasConversion(
                v => JsonConvert.SerializeObject(v,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<List<string>>(v,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })!);

        builder
            .Property(x => x.EmailsBcc)
            .HasConversion(
                v => JsonConvert.SerializeObject(v,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<List<string>>(v,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })!);

        builder
            .Property(x => x.Attachments)
            .HasConversion(
                v => JsonConvert.SerializeObject(v,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                v => JsonConvert.DeserializeObject<List<MailAttachment>>(v,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })!);
    }
}