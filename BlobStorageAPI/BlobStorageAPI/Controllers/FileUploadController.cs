using BlobStorageAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlobStorageAPI.Controllers
{
    [Route("api/v1/")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        AzureBlobService _service;
        public FileUploadController(AzureBlobService service)
        {
            _service = service;
        }

        [HttpPost("insertfile")]
        public async Task<bool> InsertFile(IFormFile asset)
        {
            try
            {
                var response = await _service.UploadFiles(asset);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        [Route("listfiles")]
        public async Task<IActionResult> GetBlobs()
        {
            var response = await _service.GetUploadedBlobs();
            return Ok(response);
        }

        [HttpGet]
        [Route("DownloadFile/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            MemoryStream ms = new MemoryStream();
            var responsefile = await _service.ReturnBlobsFile(fileName);

            if (await responsefile.ExistsAsync())
            {
                await responsefile.DownloadToStreamAsync(ms);
                Stream blobStream = responsefile.OpenReadAsync().Result;
                return File(blobStream, responsefile.Properties.ContentType, responsefile.Name);
            }
            else
            {
                return Content("File does not exist");
            }
        }

        [Route("DeleteFile/{fileName}")]
        [HttpGet]
        public async Task<bool> DeleteFile(string fileName)
        {
            try
            {
                var responsefile = await _service.ReturnBlobsFile(fileName);

                if (await responsefile.ExistsAsync())
                {
                    await responsefile.DeleteAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
