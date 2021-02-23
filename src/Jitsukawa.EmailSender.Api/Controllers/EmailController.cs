using Jitsukawa.EmailSender.Api.Models;
using Jitsukawa.EmailSender.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jitsukawa.EmailSender.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
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
        public async Task<IActionResult> Send([FromBody] EmailMessage email, [FromHeader] string Authorization)
        {
            // Resgata cliente autorizado
            var id = Authorization.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
            var customer = customerService.Get(id);

            var fails = await emailService.Send(email);

            // Concatena destinatários
            var sbAll = new StringBuilder();
            email.Recipients.ToList().ForEach(e => sbAll.Append($"{e}, "));
            var strAll = sbAll.ToString().TrimEnd(',', ' ');

            var template = $"Envio de email(s) de {customer?.Name ?? "<Desconhecido>"} para {strAll}:";

            if (fails.Length == 0)
            {
                logger.LogInformation($"{template} Nenhuma falha.");
                return Ok();
            }

            // Concatena destinatários cujo envio falhou
            var sbFails = new StringBuilder();
            fails.ToList().ForEach(r => sbFails.Append($"{r}, "));
            var strFails = sbFails.ToString().TrimEnd(',', ' ');

            logger.LogWarning($"{template} Falhou(aram) {strFails}.");
            
            return BadRequest(new { fails });
        }
    }
}
