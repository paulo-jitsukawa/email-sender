namespace Jitsukawa.EmailSender.Api.Models
{
    public class AppSettings
    {
        public string Sender { get; set; } = null!;
        public string SMTP { get; set; } = null!;
        public int Port { get; set; }
        public bool SSL { get; set; }
        public string User { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Customer[] Customers { get; set; } = new Customer[0];
    }
}
