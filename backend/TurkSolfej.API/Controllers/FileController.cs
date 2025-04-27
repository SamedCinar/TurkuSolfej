using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace TurkSolfej.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadFolder;

        public FileController(IWebHostEnvironment environment)
        {
            _environment = environment;
            _uploadFolder = Path.Combine(_environment.WebRootPath ?? "wwwroot", "uploads");
            
            // Upload klasörünü oluştur
            if (!Directory.Exists(_uploadFolder))
            {
                Directory.CreateDirectory(_uploadFolder);
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Dosya seçilmedi");
            }

            // Dosya uzantısını kontrol et
            var allowedExtensions = new[] { ".pdf", ".mp4", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest("Geçersiz dosya türü. Sadece PDF, MP4, JPG ve PNG dosyaları yüklenebilir.");
            }

            // Benzersiz dosya adı oluştur
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_uploadFolder, fileName);

            // Dosyayı kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Dosya URL'ini döndür
            var fileUrl = $"/uploads/{fileName}";
            return Ok(new { url = fileUrl });
        }

        [HttpGet("download/{fileName}")]
        public IActionResult DownloadFile(string fileName)
        {
            var filePath = Path.Combine(_uploadFolder, fileName);
            
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Dosya bulunamadı");
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), fileName);
        }

        private string GetContentType(string path)
        {
            var extension = Path.GetExtension(path).ToLowerInvariant();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".mp4" => "video/mp4",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
        }
    }
} 