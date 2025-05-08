using NAudio.Wave;
using Vosk;

namespace ConsoleBase;

internal static class Program
{
    static void Main(string[] args)
    {
        #if DEBUG
        var projectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
        #else
        var projectRoot = Path.GetFullPath(AppContext.BaseDirectory);
        #endif
        var audioPlayer = new AudioPlayer();
            
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

            Task taskPt = Task.Run(() =>
            {
                if (recognizerPt.AcceptWaveform(bufferMono, bufferMono.Length))
                {
                    var voskResultPt = new VoskResult(recognizerPt.Result());
                    voskResultPt.RemoveDiacritics();
                    Console.WriteLine(voskResultPt.Text);
            
                    if (voskResultPt.Text.Contains("invisivel", StringComparison.OrdinalIgnoreCase) ||
                        voskResultPt.Text.Contains("cobra", StringComparison.OrdinalIgnoreCase) ||
                        voskResultPt.Text.Contains("solido", StringComparison.OrdinalIgnoreCase) ||
                        voskResultPt.Text.Contains("liquido", StringComparison.OrdinalIgnoreCase))
                    {
                        audioPlayer.Play("invisible.wav");
                        ImageOpener.OpenImage("https://media.tenor.com/3t_DdMd5vYwAAAAM/metal-gear.gif");
                    } 
                    else if (voskResultPt.Text.Contains("invencivel", StringComparison.OrdinalIgnoreCase))
                    {
                        audioPlayer.Play("areyousure.wav");
                        ImageOpener.OpenImage("https://media.tenor.com/gMFriGOwj0YAAAAM/invincible-are-you-sure.gif");
                    }
                    else if (voskResultPt.Text.Contains("demonio", StringComparison.OrdinalIgnoreCase) || 
                             voskResultPt.Text.Contains("chorar", StringComparison.OrdinalIgnoreCase) ||
                             voskResultPt.Text.Contains("dante", StringComparison.OrdinalIgnoreCase))
                    {
                        audioPlayer.Play("devil.wav");
                        ImageOpener.OpenImage("https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fpbs.twimg.com%2Fmedia%2FEzbCji6UcAkiB-Q.jpg&f=1&nofb=1&ipt=cd1042e28c261d3123f6c0901cff5a4394dff62ca27f9c1b4024039c3ba96c0c");
                    }
                    else if (voskResultPt.Text.Contains("tempestade", StringComparison.OrdinalIgnoreCase) || 
                             voskResultPt.Text.Contains("luz", StringComparison.OrdinalIgnoreCase) || 
                             voskResultPt.Text.Contains("aproxima", StringComparison.OrdinalIgnoreCase) || 
                             voskResultPt.Text.Contains("vergil", StringComparison.OrdinalIgnoreCase) ||
                             voskResultPt.Text.Contains("virgil", StringComparison.OrdinalIgnoreCase))
                    {
                        audioPlayer.Play("bury.wav");
                        ImageOpener.OpenImage("https://i.kym-cdn.com/entries/icons/original/000/046/343/berried_delight.jpg");
                    }
                }
            });

            Task taskEn = Task.Run(() =>
            {
                if (recognizerEn.AcceptWaveform(bufferMono, bufferMono.Length))
                {
                    var voskResultEn = new VoskResult(recognizerEn.Result());
                    voskResultEn.RemoveDiacritics();
                    Console.WriteLine(voskResultEn.Text);

                    if (voskResultEn.Text.Contains("invisible", StringComparison.OrdinalIgnoreCase) ||
                        voskResultEn.Text.Contains("snake", StringComparison.OrdinalIgnoreCase) ||
                        voskResultEn.Text.Contains("solid", StringComparison.OrdinalIgnoreCase) ||
                        voskResultEn.Text.Contains("liquid", StringComparison.OrdinalIgnoreCase) ||
                        voskResultEn.Text.Contains("boss", StringComparison.OrdinalIgnoreCase))
                    {
                        audioPlayer.Play("invisible.wav");
                        ImageOpener.OpenImage("https://media.tenor.com/3t_DdMd5vYwAAAAM/metal-gear.gif");
                    }
                    else if (voskResultEn.Text.Contains("sure", StringComparison.OrdinalIgnoreCase) || 
                             voskResultEn.Text.Contains("invincible", StringComparison.OrdinalIgnoreCase))
                    {
                        audioPlayer.Play("areyousure.wav");
                        ImageOpener.OpenImage("https://media.tenor.com/gMFriGOwj0YAAAAM/invincible-are-you-sure.gif");
                    }
                    else if (voskResultEn.Text.Contains("devil", StringComparison.OrdinalIgnoreCase) || 
                             voskResultEn.Text.Contains("may", StringComparison.OrdinalIgnoreCase) ||
                             voskResultEn.Text.Contains("cry", StringComparison.OrdinalIgnoreCase) ||
                             voskResultEn.Text.Contains("dante", StringComparison.OrdinalIgnoreCase))
                    {
                        audioPlayer.Play("devil.wav");
                        ImageOpener.OpenImage("https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fpbs.twimg.com%2Fmedia%2FEzbCji6UcAkiB-Q.jpg&f=1&nofb=1&ipt=cd1042e28c261d3123f6c0901cff5a4394dff62ca27f9c1b4024039c3ba96c0c");
                    }
                    else if (voskResultEn.Text.Contains("bury", StringComparison.OrdinalIgnoreCase) || 
                             voskResultEn.Text.Contains("barry", StringComparison.OrdinalIgnoreCase) || 
                             voskResultEn.Text.Contains("light", StringComparison.OrdinalIgnoreCase) || 
                             voskResultEn.Text.Contains("storm", StringComparison.OrdinalIgnoreCase) ||
                             voskResultEn.Text.Contains("vergil", StringComparison.OrdinalIgnoreCase) ||
                             voskResultEn.Text.Contains("virgil", StringComparison.OrdinalIgnoreCase) ||
                             voskResultEn.Text.Contains("approaching", StringComparison.OrdinalIgnoreCase))
                    {
                        audioPlayer.Play("bury.wav");
                        ImageOpener.OpenImage("https://i.kym-cdn.com/entries/icons/original/000/046/343/berried_delight.jpg");
                    }
                }
            });
            
            Task.WaitAll(taskPt, taskEn);
        };

        waveIn.StartRecording();
        Console.WriteLine("Fale algo (Pressione Enter para encerrar)");
        Console.ReadLine();
        waveIn.StopRecording();
    }
}