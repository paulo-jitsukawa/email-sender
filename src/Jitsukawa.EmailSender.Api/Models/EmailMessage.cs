using System.ComponentModel.DataAnnotations;

namespace Jitsukawa.EmailSender.Api.Models
{
    public class EmailMessage
    {
        [Required(ErrorMessage = "Email sem assunto.")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Email sem conteúdo.")]
        public string Body { get; set; } = null!;

        [MinLength(1, ErrorMessage = "Email sem destinatário(s).")]
        public string[] Recipients { get; set; } = null!;

        public bool HTML { get; set; } = true;
    }
}
