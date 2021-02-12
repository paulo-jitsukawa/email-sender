using Jitsukawa.EmailSender.Api.Models;
using Jitsukawa.EmailSender.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Jitsukawa.EmailSender.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class EmailController : Controller
    {
        private EmailService service;

        public EmailController(EmailService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Send([FromBody] EmailMessage email)
        {
            var errors = await service.Send(email);

            if (errors.Length == 0)
            {
                return Ok();
            }

            return BadRequest(new { errors });
        }
    }
}
