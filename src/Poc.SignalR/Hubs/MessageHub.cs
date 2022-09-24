using Microsoft.AspNetCore.SignalR;
using Poc.SignalR.Hubs.Interfaces;

namespace Poc.SignalServer
{
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    // esse annotations não é necessário se "RequireAuthorization" for configurado em MapHub<THub>
    public class MessageHub : Hub<IMessage>
    {
        public async Task<string> StartConnection(string idGroup)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, idGroup);

            return Context.ConnectionId;
        }

        public async Task CloseConnectionToEspecificGroup(string idGroup)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, idGroup);

            await base.OnDisconnectedAsync(null);
        }
    }
}