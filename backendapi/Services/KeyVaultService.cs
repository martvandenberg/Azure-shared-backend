using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using backendapi.Controllers;
using Microsoft.Extensions.Logging;

namespace backendapi.Services
{
    public class KeyVaultService
    {
        private readonly ILogger<UserController> _logger;

        public KeyVaultService(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetSecret(string secretName)
        {
            var client = new SecretClient(new Uri("https://keyvaultyannickmart.vault.azure.net/"), new DefaultAzureCredential());
            KeyVaultSecret secret = await client.GetSecretAsync(secretName);
            _logger.LogInformation($"secret(value): {secret.Value} and key(name): {secret.Name}");
            //Console.WriteLine($"secret: {secret.Value}");
            //Console.WriteLine($"key: {secret.Name}");

            return secret.Value;
        }

        
    }
}
