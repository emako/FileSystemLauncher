using System.Diagnostics;

namespace FileSystemLauncher;

public class SystemProcessRunner : IProcessRunner
{
    public void Start(string fileName, string arguments)
    {
        var psi = new ProcessStartInfo(fileName, arguments)
        {
            UseShellExecute = true
        };

        Process.Start(psi);
    }
}
