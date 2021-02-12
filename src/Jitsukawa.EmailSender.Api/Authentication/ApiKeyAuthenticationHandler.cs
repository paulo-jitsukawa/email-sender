using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Jitsukawa.EmailSender.Api.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace Jitsukawa.EmailSender.Api.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string Name = "ApiKey";

        private readonly AppSettings config;

        public ApiKeyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock, IOptions<AppSettings> configuration) : base(options, logger, encoder, clock)
        {
            config = configuration.Value;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Headers.TryGetValue("Authorization", out StringValues value)) // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Authorization
            {
                var scheme = $"{Name} ";
                var header = value.FirstOrDefault();
                if (header?.StartsWith(scheme, StringComparison.OrdinalIgnoreCase) != true)
                {
                    return Task.FromResult(AuthenticateResult.NoResult());
                }

                var apiKey = header.Substring(scheme.Length);
                if (config.Customers.Any(c => c.Active && c.ApiKey == apiKey))
                {
                    return Task.FromResult(AuthenticateResult.Success(CreateAuthenticationTicket()));
                }

                return Task.FromResult(AuthenticateResult.Fail("Chave inválida."));
            }

            return Task.FromResult(AuthenticateResult.NoResult());
        }

        private AuthenticationTicket CreateAuthenticationTicket()
        {
            var claims = new[] { new Claim(ClaimTypes.Name, Name) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            return new AuthenticationTicket(principal, Scheme.Name);
        }
    }
}
