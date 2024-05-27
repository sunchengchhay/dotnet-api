using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/file/[controller]")]
    [ApiController]
    public class FileCloudController(IAmazonS3 s3Client) : ControllerBase
    {
        private readonly IAmazonS3 _s3Client = s3Client;

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] string someData)
        {
            try
            {
                //string file
                if (file == null || file.Length == 0)
                {
                    return BadRequest("File is not Selected");
                }

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var uploadRequest = new PutObjectRequest
                    {
                        BucketName = "complaint-reports",
                        Key = file.FileName,
                        InputStream = memoryStream,
                        ContentType = file.ContentType
                    };
                    await _s3Client.PutObjectAsync(uploadRequest);
                }

                var urlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = "complaint-reports",
                    Key = file.FileName,
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    Protocol = Protocol.HTTPS
                };

                var presignedUrl = _s3Client.GetPreSignedURL(urlRequest);

                var response = new
                {
                    msg = someData,
                    url = presignedUrl
                };

                return Ok(response);

            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal Server Error: ${e.Message}");
            }
        }

    }
}