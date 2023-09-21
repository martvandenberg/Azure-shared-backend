using Azure.Storage.Queues.Models;
using Flurl.Http;
using System.Reflection.Metadata;
using System.Text.Json;

namespace backendapi.Services
{
    public class LicensePlateService
    {

        string json = "https://opendata.rdw.nl/resource/m9d7-ebf2.json?kenteken=";

        public LicensePlateService()
        {
        }

        public async Task<LicensePlateJson> GetJsonAsync(string kenteken)
        {

            string url = json + kenteken;

            LicensePlateJson[] licensePlateJsonArray = await url.GetJsonAsync<LicensePlateJson[]>();
            if (licensePlateJsonArray.Length > 0)
            {
                LicensePlateJson licensePlateJson = licensePlateJsonArray[0];
                Console.WriteLine($"Kenteken: {licensePlateJson.Kenteken}, Merk: {licensePlateJson.Merk}");
                return licensePlateJson;
            }

            return null;
        }


    }

    public class LicensePlateJson
    {
        public string Kenteken { get; set; }
        public string Merk { get; set; }
    }
}
