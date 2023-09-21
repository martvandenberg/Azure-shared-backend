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
        public async Task addToQueue(UserFormModel user, string pictureUrl, string AiData)
        {
            string storageconnectionString = "DefaultEndpointsProtocol=https;AccountName=storageyannickmart;AccountKey=mCFiy7p3Lv0qwnLvo84LX21Jz/4kMV9Bh/zVKDl1drRdQJeJ5/hb0pAPS6Dz0/Xxuy/Vw6EhTLP++AStMIExNw==;EndpointSuffix=core.windows.net";
            // Instantiate a QueueClient to create and interact with the queue
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
