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
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var openIdOptions = new OpenIdOptions();
            configuration.GetSection(nameof(OpenIdOptions)).Bind(openIdOptions);

            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(openIdOptions.ClientSecret);

            services
                .AddTransient<IHttpContextAccessor, HttpContextAccessor>()
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddCookie("MyCookie", options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromSeconds(openIdOptions.AccessTokenSecondsLifetime);
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

            return services;
        }
    }
}
