namespace GenAI.API.Services
{
    public interface IVideoService
    {
        Task<string> ExtractAudioAndTranscribeAsync(string videoPath);
    }
}
