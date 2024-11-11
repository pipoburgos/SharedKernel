using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Application.Auth.Users.Commands;

/// <summary> . </summary>
public sealed record UpdateUser(Guid Id, string PhoneNumber) : ICommandRequest;
