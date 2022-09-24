using System.ComponentModel.DataAnnotations;

namespace Poc.SignalR.Models
{
    public class Login
    {
        [Required(ErrorMessage = "O campo e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail informado é inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo senha é obrigatório.")]
        [StringLength(50, ErrorMessage = "A senha deve ter entre {1} e {2} caracteres.", MinimumLength = 6)]
        public string Senha { get; set; }
    }
}
