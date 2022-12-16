using Microsoft.AspNetCore.SignalR;
using Poc.SignalR.Hubs.Interfaces;
using Poc.SignalR.Services;
using Poc.SignalR.ViewModels;

namespace Poc.SignalServer
{
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    // esse annotations não é necessário se "RequireAuthorization" for configurado em MapHub<THub>
    public class MessageHub : Hub<IMessage>
    {
        private readonly IProcessService _processService;
        private readonly IHubContext<MessageHub, IMessage> _hubContext;

        public MessageHub(IProcessService processService, IHubContext<MessageHub, IMessage> hubContext)
        {
            _processService = processService;
            _hubContext = hubContext;
        }

        public async Task<string> OpenConnection(string idGroup)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, idGroup);

            return Context.ConnectionId;
        }

        public async Task ProcessMessage(string idGroup)
        {
            var count = 0;
            MessageViewModel message;

            try
            {
                while (count < 10)
                {
                    count++;

                    Thread.Sleep(1500);

                    message = _processService.Execute(false);

                    await _hubContext
                        .Clients
                        .Group(idGroup)
                        .ProcessMessage(message);
                }

                message = _processService.Execute(true);

                await _hubContext
                    .Clients
                    .Group(idGroup)
                    .ProcessMessage(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            finally
            {
                await CloseConnection(idGroup);
            }
        }

        public async Task CloseConnection(string idGroup)
        {
            await _hubContext
                .Clients
                .Group(idGroup)
                .CloseConnection(idGroup);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, idGroup);

            await base.OnDisconnectedAsync(null);
        }
    }
}