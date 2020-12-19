using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.Application.Security;

namespace SharedKernel.Api.ServiceCollectionExtensions
{
    /// <summary>
    /// Authentication Extensions
    /// </summary>
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Configures OpenIdOptions, IHttpContextAccessor, Authentication, cookies and bearer token
        /// </summary>
        /// <param name="services">Specifies the contract for a collection of service descriptors.</param>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        /// <param name="cookieName">The cookie name. If the name is empty the cookie is not added</param>
        /// <returns></returns>
        public static IServiceCollection AddSharedKernelAuth(this IServiceCollection services, IConfiguration configuration, string cookieName = null)
        {
            var openIdOptions = new OpenIdOptions();
            configuration.GetSection(nameof(OpenIdOptions)).Bind(openIdOptions);

            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(openIdOptions.ClientSecret);

            var authenticationBuilder = services
                .AddTransient<IHttpContextAccessor, HttpContextAccessor>()
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = openIdOptions.Authority;
                    options.Audience = openIdOptions.Audience;
                    options.RequireHttpsMetadata = openIdOptions.RequireHttpsMetadata;
                    options.SaveToken = true;
                    //GetClaimsFromUserInfoEndpoint = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Validate Authority
                        ValidateIssuer = true,
                        ValidIssuer = openIdOptions.Authority,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        ValidateAudience = true,
                        ValidAudience = openIdOptions.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            if (!string.IsNullOrWhiteSpace(cookieName))
            {
                authenticationBuilder.AddCookie("MyCookie", options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(openIdOptions.AccessTokenSecondsLifetime);
                });
            }


            return services;
        }
    }
}
