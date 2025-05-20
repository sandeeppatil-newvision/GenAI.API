namespace GenAI.API.Services
{
    public interface IWhisperService
    {
        Task<string> TranscribeAudioAsync(string filePath);
    }
}
