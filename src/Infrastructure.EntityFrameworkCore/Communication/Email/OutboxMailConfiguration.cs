using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel.Application.Communication.Email;
using SharedKernel.Application.Serializers;

namespace SharedKernel.Infrastructure.EntityFrameworkCore.Communication.Email;

/// <summary>  </summary>
public class OutboxMailConfiguration : IEntityTypeConfiguration<OutboxMail>
{
    private readonly IJsonSerializer _jsonSerializer;

    /// <summary>  </summary>
    public OutboxMailConfiguration(IJsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    /// <summary>  </summary>
    public void Configure(EntityTypeBuilder<OutboxMail> builder)
    {
        builder
            .Property(x => x.To)
            .HasConversion(
                v => _jsonSerializer.Serialize(v, NamingConvention.CamelCase),
                v => _jsonSerializer.Deserialize<List<string>>(v, NamingConvention.CamelCase));

        builder
            .Property(x => x.EmailsBcc)
            .HasConversion(
                v => _jsonSerializer.Serialize(v, NamingConvention.CamelCase),
                v => _jsonSerializer.Deserialize<List<string>>(v, NamingConvention.CamelCase));

        builder
            .Property(x => x.Attachments)
            .HasConversion(
                v => _jsonSerializer.Serialize(v, NamingConvention.CamelCase),
                v => _jsonSerializer.Deserialize<List<MailAttachment>>(v, NamingConvention.CamelCase));
    }
}
