using Poc.SignalR.ViewModels;

namespace Poc.SignalR.Services
{
    public class ProcessService : IProcessService
    {
        public MessageViewModel Execute(bool processado)
        {
            if (processado)
                return new MessageViewModel("mensagem processada.");

            return new MessageViewModel("processando...");

        }
    }
}
