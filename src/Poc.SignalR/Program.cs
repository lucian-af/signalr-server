using Microsoft.AspNetCore.Http.Connections;
using Poc.SignalR.Configurations;
using Poc.SignalR.Services;
using Poc.SignalR.Settings;
using Poc.SignalServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var section = builder.Configuration.GetSection(nameof(AuthSettings));
var authSettings = section.Get<AuthSettings>();

var services = builder.Services;

services.AddScoped<IProcessService, ProcessService>();

services
    .Configure<AuthSettings>(section)
    //.AddHostedService<Worker>()
    .AddSwagger()
    .AddCors(
    options => options.AddPolicy("AllowCors",
    builder =>
    {
        builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .SetIsOriginAllowed(hostName => true);
    }))
    .AddAuthConfiguration(authSettings)
    .AddSignalR();

// ==================================================================================================================== //
var app = builder.Build();

app
    .UseCors("AllowCors")
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization()
    .UseSwagger()
    .UseSwaggerUI()
    .UseEndpoints(e =>
    {
        e.MapHub<MessageHub>("/poc", opt =>
        {
            // fecha a conexão se o token enviado estiver expirado
            opt.CloseOnAuthenticationExpiration = true;

            // limita o pedido de conexão somente por clients WebSockets
            opt.Transports = HttpTransportType.WebSockets;
        })
        // define que todos os métodos do Hub "MessageHub" precisa de autorização para aceitar conexão
        .RequireAuthorization();

        e.MapControllers();
    });

app.Run();
