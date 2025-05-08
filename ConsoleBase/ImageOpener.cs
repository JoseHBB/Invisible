using System.Diagnostics;

namespace ConsoleBase;

public class ImageOpener
{
    public static void OpenImage(string imageLink)
    {
        var psi = new ProcessStartInfo
        {
            FileName = imageLink,
            UseShellExecute = true
        };
        Process.Start(psi);
    }
}