using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Poc.SignalServer
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MessageHub : Hub<IMessage>
    {
        public async Task<string> StartConnection(string identificadorGrupo)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, identificadorGrupo);

            return Context.ConnectionId;
        }

        public async Task CloseConnection(string identificadorGrupo)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, identificadorGrupo);

            await base.OnDisconnectedAsync(null);
        }
    }
}