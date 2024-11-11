using SharedKernel.Application.Cqrs.Queries;
using System.Security.Claims;

namespace SharedKernel.Application.Auth.Users.Queries;

/// <summary> . </summary>
public sealed record GetClaims(Guid Id) : IQueryRequest<List<Claim>>;
