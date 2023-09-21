using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace backendapi.Services
{
    public class keyVaultService
    {

        public string GetSecret(string secretName)
        {
            var client = new SecretClient(new Uri("https://keyvaultyannickmart.vault.azure.net/"), new DefaultAzureCredential());
            KeyVaultSecret secret = client.GetSecret(secretName);
            Console.WriteLine($"secret: {secret.Value}");
            Console.WriteLine($"key: {secret.Name}");

            return secret.Value;
        }
        
    }
}
