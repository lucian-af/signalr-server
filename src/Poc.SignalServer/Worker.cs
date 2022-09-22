using Microsoft.AspNetCore.SignalR;

namespace Poc.SignalServer
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
            while (!stoppingToken.IsCancellationRequested)
            {
                // receber mensagem da fila
                // enviar para o client que está "esperando"

                var idPagamentoEncontrado = new Random().Next(1, 5).ToString();

                if (user is not null)
                {
                    await _hubContext
                        .Clients
                        .Group(idPagamentoEncontrado)
                        .SendMessageToEspecificClient($"Hello World!! - {count} - idPagamento: {user.IdPagamento}");

                    _logger.LogInformation("[idPagamento:{idpagamento}] - mensagem nº: {count} enviada em {now}",
                        user.IdPagamento, count, DateTime.Now);

                    count++;
                }

                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}