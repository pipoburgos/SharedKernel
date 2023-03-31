using System.Security.Claims;

namespace BankAccounts.Acceptance.Tests.Shared.Authentication
{
    public class UserMock
    {
        public UserMock(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        public ClaimsIdentity GenerateClaims(string authenticationType = "Bearer")
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.Sid, Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, authenticationType, ClaimTypes.Email, ClaimTypes.Role);
            return claimsIdentity;
        }
    }
}
