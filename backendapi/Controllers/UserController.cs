using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
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

        private readonly ILogger<UserController> _logger;
        private BlobStorageService _blobStorageService;

        public UserController(ILogger<UserController> logger, BlobStorageService blobService)
        {
            _logger = logger;
            _blobStorageService = blobService;
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
