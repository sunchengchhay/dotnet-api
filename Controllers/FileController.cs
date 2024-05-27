using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("[controller]")]
    public class FileController : Controller
    {
        private readonly ILogger<FileController> _logger;

        public FileController(ILogger<FileController> logger)
        {
            _logger = logger;
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile([FromForm] FileUpload model)
        {
            if (model.File == null && model.File.Length == 0)
            {
                return BadRequest("Invalid File");
            }

            var folderName = Path.Combine("Resources", "AllFiles");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            var fileName = model.File.FileName;
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);

            if (System.IO.File.Exists(fullPath))
            {
                return BadRequest("file already exists");
            }

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                model.File.CopyTo(stream);
            }

            return Ok(new { dbPath });
        }

        [HttpPost("multipleupload"), DisableRequestSizeLimit]
        public async Task<IActionResult> MultipleUploadFile([FromForm] MultipleFileUpload model)
        {
            var response = new Dictionary<string, string>();
            if (model.Files == null && model.Files.Count == 0)
            {
                return BadRequest("Invalid File");
            }

            foreach (var file in model.Files)
            {
                var folderName = Path.Combine("Resources", "AllFiles");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                var fileName = file.FileName;
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine(folderName, fileName);

                if (!System.IO.File.Exists(fullPath))
                {
                    using var memoryStream = new MemoryStream();
                    await file.CopyToAsync(memoryStream);
                    await System.IO.File.WriteAllBytesAsync(fullPath, memoryStream.ToArray());
                    response.Add(fileName, dbPath);
                }
                else
                {
                    response.Add(fileName, "already exists");
                }
            }

            return Ok(new { response });
        }

        [HttpGet("download/{name}")]
        public async Task<IActionResult> DownloadByName(string name)
        {
            var folderName = Path.Combine("Resources", "AllFiles");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fileName = name;
            var fullPath = Path.Combine(pathToSave, fileName);
            if (!System.IO.File.Exists(fullPath))
            {
                return BadRequest("file not exists");
            }
            var fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
            var fileContentResult = new FileContentResult(fileBytes, "application/octet-stream")
            {
                FileDownloadName = fileName
            };
            return fileContentResult;
        }

    }
}