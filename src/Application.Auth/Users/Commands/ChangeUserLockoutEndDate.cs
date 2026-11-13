using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Application.Auth.Users.Commands;

public sealed record ChangeUserLockoutEndDate(Guid UserId, DateTime? LockoutEndDate) : ICommandRequest;