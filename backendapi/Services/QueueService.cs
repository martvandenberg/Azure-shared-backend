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
        private readonly keyVaultService _keyVaultService;
        public QueueService(keyVaultService keyVaultService) {
            _keyVaultService = keyVaultService;
        }
        public async Task addToQueue(UserFormModel user, string pictureUrl, string AiData)
        {
            string storageconnectionString = _keyVaultService.GetSecret("storageaccountconnectionstring");
            QueueClient queueClient = new QueueClient(storageconnectionString, "personen");
            string jsonString = JsonSerializer.Serialize(
                    new QueueItem(
                        user.FirstName,
                        user.LastName,
                        user.LicensePlate,
                        pictureUrl,
                        AiData
                    )
                );
            await queueClient.SendMessageAsync(jsonString);
        }
    }

    public record QueueItem(
        string FirstName,
        string LastName,
        string LicencePlate,
        string ImageUrl,
        string AnalysisResult
    );
}
