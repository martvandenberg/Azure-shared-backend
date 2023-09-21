using Azure.Identity;
using Azure.Storage.Queues;
using Microsoft.Azure.Cosmos;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Text.Json;
using static backendapi.Controllers.UserController;

namespace backendapi.Services
{
    public class QueueService
    {
        private readonly KeyVaultService _keyVaultService;
        public QueueService(KeyVaultService keyVaultService) {
            _keyVaultService = keyVaultService;
        }
        public async Task addToQueue(UserFormModel user, string pictureUrl, string AiData, LicensePlateJson licensePlateJson)
        {
            string storageconnectionString = await _keyVaultService.GetSecret("storageaccountconnectionstring");

            QueueClient queueClient = new QueueClient(storageconnectionString, "personen", new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });
            string jsonString = JsonSerializer.Serialize(
                    new QueueItem(
                        user.FirstName,
                        user.LastName,
                        user.LicensePlate,
                        pictureUrl,
                        AiData,
                        licensePlateJson.Merk
                    )
                );
            await queueClient.SendMessageAsync(jsonString);
        }
    }

    public record QueueItem(
        string FirstName,
        string LastName,
        string LicensePlate,
        string ImageUrl,
        string AnalysisResult,
        string MerkLicensePlate
    );
}
