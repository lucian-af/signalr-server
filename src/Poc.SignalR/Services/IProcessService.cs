using Poc.SignalR.ViewModels;

namespace Poc.SignalR.Services
{
    public interface IProcessService
    {
        MessageViewModel Execute(bool processado);
    }
}
