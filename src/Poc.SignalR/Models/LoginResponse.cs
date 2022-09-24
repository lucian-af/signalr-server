namespace Poc.SignalR.Models
{
    public class LoginResponse
    {
        public string Token { get; set; }

        public int ExpireIn { get; set; }

        public UsuarioLogado Usuario { get; set; }
    }

    public class UsuarioLogado
    {
        public string Email { get; set; }
    }
}
