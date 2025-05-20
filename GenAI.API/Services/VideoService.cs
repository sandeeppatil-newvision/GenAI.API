using Xabe.FFmpeg;

namespace GenAI.API.Services
{
    public class VideoService : IVideoService
    {
        private readonly IWhisperService _whisperService;

        public VideoService(IWhisperService whisperService)
        {
            _whisperService = whisperService;
            // Ensure FFmpeg executables path is set correctly
            var ffmpegPath = "C:\\ffmpeg-7.1.1-essentials_build\\ffmpeg-7.1.1-essentials_build\\bin"; // Replace with the actual path to FFmpeg executables
            if (string.IsNullOrWhiteSpace(ffmpegPath) || !Directory.Exists(ffmpegPath))
            {
                throw new DirectoryNotFoundException("Please add FFmpeg executables to your PATH variable or specify a valid directory path.");
            }

            FFmpeg.SetExecutablesPath(ffmpegPath);
        }

        public async Task<string> ExtractAudioAndTranscribeAsync(string videoPath)
        {
            try
            {
                var audioPath = Path.ChangeExtension(videoPath, ".mp3");
                var conversion = await FFmpeg.Conversions.FromSnippet.ExtractAudio(videoPath, audioPath);
                await conversion.Start();
                return await _whisperService.TranscribeAudioAsync(audioPath);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
    }
}
