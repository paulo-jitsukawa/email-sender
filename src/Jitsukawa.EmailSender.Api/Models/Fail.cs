using System;

namespace Jitsukawa.EmailSender.Api.Models
{
    /// <summary>
    /// Falha total ou parcial prevista pela aplicação.
    /// </summary>
    public class Fail : Exception
    {
        /// <param name="message">Mensagem para o usuário.</param>
        /// <param name="inner">Exceção para fins de depuração.</param>
        public Fail(string message, Exception? inner = null) : base(message, inner) { }
    }
}
