namespace GenAI.API.Models
{
    public class UploadRequest
    {
        public IFormFile File { get; set; }
        public string OutputType { get; set; } // e.g., "test-case" or "user-story"
    }
}
