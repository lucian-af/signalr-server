using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Poc.SignalR.Settings;
using System.Text;

namespace Poc.SignalR.Configurations
{
    public static class AuthConfiguration
    {
        private const string AUTH_SECRET = "AUTH_SECRET";

        public static IServiceCollection AddAuthConfiguration(this IServiceCollection services, AuthSettings authSettings)
        {
            var secret = Environment.GetEnvironmentVariable(AUTH_SECRET);
            var key = Encoding.ASCII.GetBytes(secret);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
                {
                    x.RequireHttpsMetadata = true;
                    x.SaveToken = true;

                    x.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = authSettings.ValidoEm,
                        ValidIssuer = authSettings.Emissor
                    };
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/poc")))
                            {
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
