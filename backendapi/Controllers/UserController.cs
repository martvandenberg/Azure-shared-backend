using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using backendapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace backendapi.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : Controller
    {
        private readonly AnalyseImage _analyseImage;
        private readonly ILogger<UserController> _logger;
        private BlobStorageService _blobStorageService;
        private readonly QueueService _queueService;
        private readonly keyVaultService _keyVaultService;
        //Update the storageAccountName value that you recorded previously in this lab.
        private string storageAccountName = "storageyannickmart";

        //Update the storageAccountKey value that you recorded previously in this lab.
        private string storageAccountKey = "mCFiy7p3Lv0qwnLvo84LX21Jz/4kMV9Bh/zVKDl1drRdQJeJ5/hb0pAPS6Dz0/Xxuy/Vw6EhTLP++AStMIExNw==";

        public UserController(ILogger<UserController> logger, BlobStorageService blobService, AnalyseImage analyseImage, QueueService queueService, keyVaultService keyVaultService)
        {
            _analyseImage = analyseImage;
            _logger = logger;
            _blobStorageService = blobService;
            _queueService = queueService;
            _keyVaultService = keyVaultService;
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] UserFormModel userForm)
        {
            _logger.LogInformation($"LOG INFO. username: {userForm.FirstName} and lastname {userForm.LastName}");
            //_logger.LogInformation($"Header: {HttpContext.User}");
            Console.WriteLine($"First Name: {userForm.FirstName}");
            Console.WriteLine($"Last Name: {userForm.LastName}");
            Console.WriteLine($"License Plate: {userForm.LicensePlate}");
            if (userForm.Picture != null)
            {
                await _blobStorageService.UploadImage(userForm.Picture);
                var client = _blobStorageService.getBlobClient(userForm.Picture);
                string uriSassToken = _blobStorageService.GenerateSasToken(client);

                string analysis = _analyseImage.AnalyseImageWithAi(uriSassToken);

                await _queueService.addToQueue(userForm, client.Uri.ToString(), analysis);
            } else
            {
                Console.WriteLine("picture is null");
            }            

            return Ok(new { message = "Data received" });
        }

        public class UserFormModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string LicensePlate { get; set; }
            public IFormFile? Picture { get; set; }
        }

        



    }
}
