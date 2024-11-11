using SharedKernel.Application.Cqrs.Queries;

namespace SharedKernel.Application.Auth.Users.Queries;

/// <summary> . </summary>
public sealed record GetRoles(Guid Id) : IQueryRequest<List<string>>;
