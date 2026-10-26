using SharedKernel.Application.Cqrs.Commands;

namespace SharedKernel.Application.Auth.Users.Commands;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
public sealed record ReactivateAuthUser(Guid Id) : ICommandRequest;
