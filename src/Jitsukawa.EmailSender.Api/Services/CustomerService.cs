using Jitsukawa.EmailSender.Api.Models;
using Microsoft.Extensions.Options;
using System.Linq;

namespace Jitsukawa.EmailSender.Api.Services
{
    public class CustomerService
    {
        private readonly AppSettings settings;

        public CustomerService(IOptions<AppSettings> configuration)
        {
            settings = configuration.Value;
        }

        public Customer? Get(string id)
        {
            return settings.Customers.FirstOrDefault(c => c.ApiKey == id);
        }
    }
}
