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
        private readonly LicensePlateService _licensePlateService;
        private readonly AnalyseImage _analyseImage;
        private readonly ILogger<UserController> _logger;
        private BlobStorageService _blobStorageService;
        private readonly QueueService _queueService;
        private readonly KeyVaultService _keyVaultService;
        //Update the storageAccountName value that you recorded previously in this lab.
        private string storageAccountName = "storageyannickmart";

        //Update the storageAccountKey value that you recorded previously in this lab.
        private string storageAccountKey = "mCFiy7p3Lv0qwnLvo84LX21Jz/4kMV9Bh/zVKDl1drRdQJeJ5/hb0pAPS6Dz0/Xxuy/Vw6EhTLP++AStMIExNw==";

        public UserController(ILogger<UserController> logger, BlobStorageService blobService, AnalyseImage analyseImage, QueueService queueService, KeyVaultService keyVaultService, LicensePlateService licensePlateService)
        {
            _licensePlateService = licensePlateService;
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
            LicensePlateJson licenseplate = await _licensePlateService.GetJsonAsync(userForm.LicensePlate);
            string analysis = "";
            if (userForm.Picture != null)
            {
                await _blobStorageService.UploadImage(userForm.Picture);
                var client = _blobStorageService.getBlobClient(userForm.Picture);
                string uriSassToken = _blobStorageService.GenerateSasToken(client);

                analysis = _analyseImage.AnalyseImageWithAi(uriSassToken);

                await _queueService.addToQueue(userForm, client.Uri.ToString(), analysis, licenseplate);
            } else
            {
                Console.WriteLine("picture is null");
            }

            AnalysisWithLicenseInfo analysisWithLicenseInfo;
            if (licenseplate.Merk != null)
            {
                analysisWithLicenseInfo = new AnalysisWithLicenseInfo(analysis, licenseplate.Merk);
                return Ok( analysisWithLicenseInfo );
            }
            else
            {
                analysisWithLicenseInfo = new AnalysisWithLicenseInfo(analysis, "");
                return Ok(analysisWithLicenseInfo);
            }

        }

        public record AnalysisWithLicenseInfo(
            string analysis,
            string autoMerk
        );

        public class UserFormModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string LicensePlate { get; set; }
            public IFormFile? Picture { get; set; }
        }

        



    }
}
