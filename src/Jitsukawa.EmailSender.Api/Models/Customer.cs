namespace Jitsukawa.EmailSender.Api.Models
{
    public class Customer
    {
        public string Name { get; set; } = null!;
        public string ApiKey { get; set; } = null!;
        public bool Active { get; set; } = true;
    }
}
