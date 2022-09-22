using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Poc.SignalServer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var token = "35bvfds(G+KbPeShVkYp4s";
var key = Encoding.ASCII.GetBytes(token);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false
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

builder.Services.AddSignalR();
builder.Services.AddHostedService<Worker>();

builder.Services.AddCors(
    options => options.AddPolicy("AllowCors",
    builder =>
    {
        builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(hostName => true);
    }));

// ==================================================================================================================== //
var app = builder.Build();
app.UseCors("AllowCors");
app.UseAuthorization();

app.MapHub<MessageHub>("/poc");
app.Run();
