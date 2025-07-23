using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CitysInfo.Controllers
{

    [ApiController]
    [Route("api/files")]
    public class FilesController : Controller
    {


        private FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider ?? throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("{fileId}")]
        public IActionResult Getfile(int fileId)
        {
            var pathToFile = "PP00101.pdf";
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            if(!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile,out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }

        [HttpPost]

        public async Task<ActionResult> CreateFile(IFormFile file)
        {
            //validate input file and check for file size for any large file attacks
            //only accept pdf files
            if(file.Length == 0 || file.Length >= 20971520 || file.ContentType != "application/pdf")
            {
                return BadRequest("No File or Invalid content is been Inputted.");
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(),$"uploaded_file_{Guid.NewGuid()}.pdf");

            using(var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("Your File has been created successfully");
        }
    }
}
