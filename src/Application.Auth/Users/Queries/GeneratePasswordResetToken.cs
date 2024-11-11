using SharedKernel.Application.Cqrs.Queries;

namespace SharedKernel.Application.Auth.Users.Queries;

/// <summary> . </summary>
public sealed record GeneratePasswordResetToken(string Email) : IQueryRequest<string>;
