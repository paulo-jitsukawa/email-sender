using Jitsukawa.EmailSender.Api;
using Jitsukawa.EmailSender.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Jitsukawa.EmailSender.Test.IntegrationTests
{
    public class EmailTests : IntegrationTest
    {
        public EmailTests(WebAppFactory<Startup> factory) : base(factory) { }

#warning Substitua "email_de_teste@provedor.com.br" por um endereço válido seu. 
        private const string ValidEmailAdress = "email_de_teste@provedor.com.br";
        private const string InvalidEmailAdress = "endereco.invalido";

        /// <summary>
        /// Testa se a aplicação rejeita mensagem válida de clientes inválidos.
        /// </summary>
        /// <param name="key">Chave inválida ou válida para cliente não autorizado.</param>
        [Theory]
        [InlineData("10785c29dabba06fc73876564d7a5624")]  // Cliente desconhecido
        [InlineData("c0388527509c44d9a694aae0104718ae")]  // Cliente desativado
        public async Task SendToUnauthorizedClient(string key)
        {
            var client = CreateClient(key);

            var message = new EmailMessage
            {
                Subject = "Teste",
                Recipients = new string[] { ValidEmailAdress },
                Body = "Teste",
                HTML = false
            };

            var json = JsonSerializer.Serialize(message);

            var result = await Post(json, "api/v1/email", client);

            Assert.Equal(HttpStatusCode.Unauthorized, result.Code);
        }

        /// <summary>
        /// Testa se a aplicação rejeita mensagens inválidas de cliente válido.
        /// </summary>
        /// <param name="json">Mensagens em Json inválidas.</param>
        [Theory]
        [InlineData("")]
        [InlineData("{\"subject\": \"Teste\", \"recipients\": [\"" + ValidEmailAdress + "\"]}")]
        [InlineData("{\"subject\": \"Teste\", \"body\": \"Teste\"}")]
        [InlineData("{\"recipients\": [\"" + ValidEmailAdress + "\"], \"body\": \"Teste\"}")]
        public async Task SendInvalidMessage(string json)
        {
            var client = CreateClient("c20c5ea0964b46a284f60217a2285000");

            var result = await Post(json, "api/v1/email", client);

            Assert.NotEqual(HttpStatusCode.OK, result.Code);
        }

        /// <summary>
        /// Testa envio de mensagem válida de cliente válido para destinatários válido e inválido. 
        /// </summary>
        [Fact]
        public async Task SendToMultipleRecipients()
        {
            var client = CreateClient("8bb2c0e5dd2142c1957094b4670ebc2a");

            var message = new EmailMessage
            {
                Subject = "Teste",
                Recipients = new string[] { InvalidEmailAdress, ValidEmailAdress },
                Body = "Favor ignorar esta mensagem. Ela foi enviada com o objetivo de validar nossos recursos computacionais.",
                HTML = false
            };

            var json = JsonSerializer.Serialize(message);

            var result = await Post(json, "api/v1/email", client);

            Assert.Equal(HttpStatusCode.BadRequest, result.Code);
            Assert.Contains("\"status\":422", result.Response);
            Assert.Contains(InvalidEmailAdress, result.Response);
            Assert.DoesNotContain(ValidEmailAdress, result.Response);
        }
    }
}
