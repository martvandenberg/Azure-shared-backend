using Azure.Storage.Blobs;
using Azure.Storage;

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
            var blobClient = containerClient.GetBlobClient(Picture.FileName);

            using (var stream = Picture.OpenReadStream())
            {
                var res = await blobClient.UploadAsync(stream);
            }
        }
    }
}
