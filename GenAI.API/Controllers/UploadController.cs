using GenAI.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace GenAI.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController : ControllerBase
    {

        private readonly IWhisperService _whisperService;
        private readonly IVideoService _videoService;
        private readonly IOpenAiService _openAiService;

        public UploadController(IWhisperService whisperService, IVideoService videoService, IOpenAiService openAiService)
        {
            _whisperService = whisperService;
            _videoService = videoService;
            _openAiService = openAiService;
        }
        [HttpGet]
        public string GetData()
        {
            return "Hello, this is the AssetsController!";
        }

        [HttpPost]
        [RequestSizeLimit(104857600)]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string outputType)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var filePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + extension);

            using (var stream = new FileStream(filePath, FileMode.Create))
                await file.CopyToAsync(stream);

            string extractedText = extension switch
            {
                ".txt" => await System.IO.File.ReadAllTextAsync(filePath),
                ".mp3" or ".wav" => await _whisperService.TranscribeAudioAsync(filePath),
                ".mp4" => await _videoService.ExtractAudioAndTranscribeAsync(filePath),
                _ => throw new NotSupportedException("Unsupported file type")
            };

            var content = await _openAiService.GenerateFormattedContentAsync(extractedText, outputType);
            return Ok(new { content });
        }
    }
}
