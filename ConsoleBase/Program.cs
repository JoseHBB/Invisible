using NAudio.Wave;
using Vosk;

namespace ConsoleBase;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Current Directory: " + Environment.CurrentDirectory);

        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        var modelPathPt = Path.Combine(projectRoot, "models", "vosk-pt");
        var modelPathEn = Path.Combine(projectRoot, "models", "vosk-en");

        Console.WriteLine("Inicializando modelos...");

        Vosk.Vosk.SetLogLevel(0);

        var modelPt = new Model(modelPathPt);
        var recognizerPt = new VoskRecognizer(modelPt, 16000);

        var modelEn = new Model(modelPathEn);
        var recognizerEn = new VoskRecognizer(modelEn, 16000);

        using var waveIn = new WaveInEvent();
        waveIn.WaveFormat = new WaveFormat(16000, 16, 1); // Captura 2 canais, 16bits

        int channels = waveIn.WaveFormat.Channels;
        int bytesPerSample = waveIn.WaveFormat.BitsPerSample / 8;
            
        waveIn.DataAvailable += (s, e) =>
        {
            byte[] bufferMono;
            if (channels == 2)
            {
                int samples = e.BytesRecorded / (bytesPerSample * channels);
                bufferMono = new byte[samples * bytesPerSample];
                for (int i = 0; i < samples; i++)
                {
                    // Copia só o canal esquerdo (primeiros dois bytes do frame)
                    bufferMono[i * bytesPerSample] = e.Buffer[i * bytesPerSample * 2];
                    bufferMono[i * bytesPerSample + 1] = e.Buffer[i * bytesPerSample * 2 + 1];
                }
            }
            else
            {
                bufferMono = new byte[e.BytesRecorded];
                Array.Copy(e.Buffer, bufferMono, e.BytesRecorded);
            }

            if (recognizerPt.AcceptWaveform(bufferMono, bufferMono.Length))
                Console.WriteLine(recognizerPt.Result());
                
            if (recognizerEn.AcceptWaveform(bufferMono, bufferMono.Length))
                Console.WriteLine(recognizerEn.Result());
        };

        waveIn.StartRecording();
        Console.WriteLine("Fale algo (Pressione Enter para encerrar)");
        Console.ReadLine();
        waveIn.StopRecording();
    }
}