namespace Poc.SignalServer
{
    public interface IMessage
    {
        Task SendMessageToEspecificClient(string message);
    }
}