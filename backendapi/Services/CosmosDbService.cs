using Azure.Identity;
using Microsoft.Azure.Cosmos;
using System.Collections.Concurrent;
using System.ComponentModel;
using static backendapi.Controllers.UserController;

namespace backendapi.Services
{
    public class CosmosDbService
    {
        public async Task addToDB(UserFormModel user)
        {
        //    using CosmosClient client = new CosmosClient("AccountEndpoint=https://yannickmart.documents.azure.com:443/;AccountKey=PZRRBvIx2drTZNL5K0v4ZTG8z8mjRxevJDTcvzck4uM7Lt1z8G1EXGwecAdyy6m2XfZci24JTF8JACDbgyM91Q==;");

        //    Database database = await client.CreateDatabaseIfNotExistsAsync(id: "Personen");
        //    Microsoft.Azure.Cosmos.Container container = database.GetContainer(id: "Personen");
        //    Persoon createdItem = await container.CreateItemAsync(
        //    item: new(){
        //        id = Guid.NewGuid().ToString(),
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        LicencePlate = user.LicensePlate
        //    }
        //);
        }
    }

    public record Persoon(
    string id,
    string FirstName
    );
}
