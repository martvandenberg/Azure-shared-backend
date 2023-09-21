using System;
using System.Collections.Generic;
using System.Text.Json;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace yannickmartfunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.PostConfigure<QueueTriggerAttribute>(x =>
            {
                var client = new SecretClient(new Uri("https://keyvaultyannickmart.vault.azure.net/"), new DefaultAzureCredential());
                KeyVaultSecret secret = client.GetSecret("storageaccountconnectionstring");
                x.Connection = secret.Value;
            });
        }
    }
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([QueueTrigger("personen", Connection = "")]string myQueueItem, [CosmosDB(
                databaseName: "Personen",
                containerName: "Personen",
                Connection = "ConnectionStrings:cosmosdb")]out dynamic document, ILogger log)
        {
            Persoon? persoon = JsonSerializer.Deserialize<Persoon>(myQueueItem);
            document = new { Id = Guid.NewGuid().ToString(), persoon };
            log.LogInformation($"Document: {document}");
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
    public class Persoon
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicensePlate { get; set; }
        public string ImageUrl { get; set; }
        public string AnalysisResult { get; set; }

    }
}
