namespace Poc.SignalR.Hubs.Interfaces
{
    public interface IMessage
    {
        Task SendMessageToEspecificGroup(string message);
        Task CloseConnectionToEspecificGroup(string idGroup);
    }
}