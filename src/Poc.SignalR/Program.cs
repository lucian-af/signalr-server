using Poc.SignalR.BackgroundTask;
using Poc.SignalR.Configurations;
using Poc.SignalR.Settings;
using Poc.SignalServer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var authSettings = builder.Configuration.GetSection(nameof(AuthSettings));
builder.Services.Configure<AuthSettings>(authSettings);

builder.Services
    .AddSwagger()
    .AddHostedService<Worker>()
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
    .AddAuthConfiguration()
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
            opt.CloseOnAuthenticationExpiration = true;
        });
        e.MapControllers();
    });

app.Run();
