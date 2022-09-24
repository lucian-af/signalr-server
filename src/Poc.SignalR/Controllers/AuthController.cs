using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Poc.SignalR.Models;
using Poc.SignalR.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Poc.SignalR.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthSettings _authSettings;
        private const string AUTH_SECRET = "AUTH_SECRET";

        public AuthController(IOptions<AuthSettings> authSettings)
            => _authSettings = authSettings.Value;

        [HttpPost]
        [Route("autenticar")]
        [AllowAnonymous]
        public IActionResult Autenticar(Login request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dados inválidos.");

            var token = GerarToken(request);

            if (string.IsNullOrWhiteSpace(token))
                return BadRequest("Falha ao gerar o token.");

            var usuarioLogado = new LoginResponse
            {
                Token = token,
                ExpireIn = Convert.ToInt16(TimeSpan.FromHours(_authSettings.ExpiracaoHoras).TotalSeconds),
                Usuario = new UsuarioLogado
                {
                    Email = request.Email
                }
            };

            return Created("", usuarioLogado);
        }

        private string GerarToken(Login dadosLogin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable(AUTH_SECRET));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _authSettings.Emissor,
                Audience = _authSettings.ValidoEm,
                Expires = DateTime.UtcNow.AddHours(_authSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email, dadosLogin.Email),
                    new Claim("role", "Administrator")
                })
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }
    }
}