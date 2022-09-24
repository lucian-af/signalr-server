namespace Poc.SignalR.Hubs.Interfaces
{
    public interface IMessage
    {
        Task SendMessageToEspecificClient(string message);
    }
}