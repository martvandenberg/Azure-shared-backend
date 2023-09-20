using Azure.Storage.Blobs;
using Azure.Storage;
using Microsoft.AspNetCore.Mvc.Rendering;
using Azure.Storage.Sas;
using Azure.Storage.Blobs.Models;

namespace backendapi
{
    public class BlobStorageService
    {
        private string blobServiceEndpoint = "https://storageyannickmart.blob.core.windows.net/";

        //Update the storageAccountName value that you recorded previously in this lab.
        private string storageAccountName = "storageyannickmart";

        //Update the storageAccountKey value that you recorded previously in this lab.
        private string storageAccountKey = "mCFiy7p3Lv0qwnLvo84LX21Jz/4kMV9Bh/zVKDl1drRdQJeJ5/hb0pAPS6Dz0/Xxuy/Vw6EhTLP++AStMIExNw==";
        public async Task UploadImage(IFormFile Picture)
        {
            Console.WriteLine($"Picture Length: {Picture.Length}");
            StorageSharedKeyCredential accountCredentials = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);
            BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), accountCredentials);
            var containerClient = blobServiceClient.GetBlobContainerClient("image");
            //string randomkey = new Guid().ToString();
            var blobClient = containerClient.GetBlobClient(Picture.FileName);

            BlobUploadOptions options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = "image/jpeg",
                    ContentDisposition = "inline; filename=YourImageName.jpg"
                }
            };

            using (var stream = Picture.OpenReadStream())
            {
                var res = await blobClient.UploadAsync(stream, options);
            }

        }

        public BlobClient getBlobClient(IFormFile Picture)
        {
            StorageSharedKeyCredential accountCredentials = new StorageSharedKeyCredential(storageAccountName, storageAccountKey);
            BlobServiceClient blobServiceClient = new BlobServiceClient(new Uri(blobServiceEndpoint), accountCredentials);
            var containerClient = blobServiceClient.GetBlobContainerClient("image");
            return containerClient.GetBlobClient(Picture.FileName);
        }

        public string GenerateSasToken(BlobClient blobClient)
        {
            var sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1), // 1 hour expiry
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(storageAccountName, storageAccountKey)).ToString();

            return $"{blobClient.Uri}?{sasToken}";
        }
    }
}
