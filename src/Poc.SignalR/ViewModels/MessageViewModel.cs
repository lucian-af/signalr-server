namespace Poc.SignalR.ViewModels
{
    public class MessageViewModel
    {
        public MessageViewModel(string content)
        {
            Content = content;
        }

        public string Content { get; private set; }
    }
}