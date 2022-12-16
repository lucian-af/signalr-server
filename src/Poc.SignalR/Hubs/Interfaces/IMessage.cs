using Poc.SignalR.ViewModels;

namespace Poc.SignalR.Hubs.Interfaces
{
    public interface IMessage
    {
        Task ProcessMessage(MessageViewModel message);
        Task CloseConnection(string idGroup);
    }
}