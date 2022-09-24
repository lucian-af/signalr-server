using Microsoft.AspNetCore.SignalR;
using Poc.SignalR.Hubs.Interfaces;
using Poc.SignalServer;

namespace Poc.SignalR.BackgroundTask
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHubContext<MessageHub, IMessage> _hubContext;

        public Worker(IHubContext<MessageHub, IMessage> hubContext, ILogger<Worker> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker em execução - {data}", DateTime.Now);

            var count = 0;
            while (!stoppingToken.IsCancellationRequested && count < 11)
            {
                var messageGroupQueue = 1;

                await _hubContext
                    .Clients
                    .Group(messageGroupQueue.ToString())
                    .SendMessageToEspecificGroup($"Hello World!! - {count} - group: {messageGroupQueue}");

                _logger.LogInformation("[messageQueue:{messageQueue}] - mensagem nº: {count} enviada em {now}",
                    messageGroupQueue, count, DateTime.Now);

                if (count == 10)
                {
                    await _hubContext
                        .Clients
                        .Group(messageGroupQueue.ToString())
                        .CloseConnectionToEspecificGroup(messageGroupQueue.ToString());
                }

                count++;

                await Task.Delay(2000, stoppingToken);
            }

            _logger.LogInformation("Worker encerrado em: {data}", DateTime.Now);
        }
    }
}