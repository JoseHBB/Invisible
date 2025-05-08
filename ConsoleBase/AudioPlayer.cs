using NAudio.Wave;

namespace ConsoleBase;

public class AudioPlayer
{
    #if DEBUG
    private static readonly string ProjectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
    #else
    private static readonly string ProjectRoot = Path.GetFullPath(AppContext.BaseDirectory);
    #endif
    
    private readonly WaveOutEvent _outputDevice = new WaveOutEvent();
    private AudioFileReader _audioFileReader = new AudioFileReader(Path.Combine(ProjectRoot, "assets", "audio", "invisible.wav"));

    public void Play(string audioFileName)
    {
        _outputDevice.Stop();
        _outputDevice.Dispose();
        _audioFileReader.Dispose();
        _audioFileReader = new AudioFileReader(Path.Combine(ProjectRoot, "assets", "audio", audioFileName));
        _outputDevice.Init(_audioFileReader);
        _outputDevice.Play();
    }
}