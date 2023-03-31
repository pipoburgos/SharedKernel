using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace BankAccounts.Acceptance.Tests.Shared.Authentication
{
    /// <summary>
    /// Allow to set userName, roles or anything useful for faking a user
    /// </summary>
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Define a Token with a custom object
        /// </summary>
        /// <param name="client"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static HttpClient SetFakeBearerToken(this HttpClient client, object token)
        {
            client.SetToken(FakeJwtBearerDefaults.AuthenticationScheme, JsonConvert.SerializeObject(token));

            return client;
        }

        /// <summary>
        /// Define a Token with juste a Username
        /// </summary>
        /// <param name="client"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static HttpClient SetFakeBearerToken(this HttpClient client, string userName)
        {
            client.SetFakeBearerToken(new
            {
                sub = userName
            });

            return client;
        }

        /// <summary>
        /// Define a Token with a Username and some roles
        /// </summary>
        /// <param name="client"></param>
        /// <param name="userName"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public static HttpClient SetFakeBearerToken(this HttpClient client, string userName, string[] roles)
        {

            client.SetFakeBearerToken(new
            {
                sub = userName,
                role = roles
            });

            return client;
        }

        /// <summary>
        /// Define a Token with a Username and some roles and otherclaim
        /// </summary>
        /// <param name="client"></param>
        /// <param name="contexto"></param>
        /// <returns></returns>
        public static HttpClient SetFakeBearerToken(this HttpClient client, UserMock contexto)
        {
            //claim.id = Guid.NewGuid();
            //claim.sub = userName;
            //claim.role = roles;

            client.SetFakeBearerToken(contexto.GenerateClaims().Claims.GroupBy(a => a.Type).ToDictionary(a => a.Key, b => b.Select(a => a.Value)));

            return client;
        }

        /// <summary>
        /// Set Raw Token
        /// </summary>
        /// <param name="client"></param>
        /// <param name="scheme"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static HttpClient SetToken(this HttpClient client, string scheme, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);

            return client;
        }
    }
}
