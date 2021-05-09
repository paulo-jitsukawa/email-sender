using Jitsukawa.EmailSender.Api.Models;
using Jitsukawa.EmailSender.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jitsukawa.EmailSender.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class EmailController : Controller
    {
        private EmailService emailService;
        private CustomerService customerService;
        private ILogger logger;

        public EmailController(EmailService email, CustomerService customer, ILogger<EmailController> logger)
        {
            emailService = email;
            customerService = customer;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromHeader] string Authorization, [FromBody] EmailMessage email)
        {
            // Resgata cliente autorizado
            var id = Authorization.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
            var customer = customerService.Get(id);
            if (customer == null)
            {
                return Unauthorized();
            }

            try
            {
                await emailService.Send(email);

                // Monta e registra log de informação contendo destinatários da mensagem
                var log = $"Cliente {customer.Name} enviou emails para: ";
                email.Recipients.ToList().ForEach(r => log += $"{r}, ");
                logger.LogInformation(log.TrimEnd(' ', ','));

                return Ok();
            }
            catch (Fail f)
            {
                // Monta e registra log de alerta contendo destinatários da mensagem
                var log = $"Cliente {customer.Name} tentou enviar emails para: ";
                email.Recipients.ToList().ForEach(r => log += $"{r}, ");
                logger.LogWarning($"{log.TrimEnd(' ', ',')}. {f.Message}");

                return BadRequest(new
                {
                    type = "https://developer.mozilla.org/pt-BR/docs/Web/HTTP/Status/422",
                    title = "One or more emails have not been sent.",
                    status = 422,
                    errors = f.Message.Split(':')[1].Trim(' ', '.').Split(',')
                });
            }
            catch (Exception e)
            {
                logger.LogError($"Cliente {customer.Name} tentou enviar emails: {e.Message}.{Environment.NewLine}{e.StackTrace}");

                return Problem(statusCode: 500);
            }
        }
    }
}
