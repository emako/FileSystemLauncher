using System.Diagnostics;

namespace FileSystemLauncher;

public class SystemProcessRunner : IProcessRunner
{
    public void Start(string fileName, string arguments)
    {
        ProcessStartInfo psi = new(fileName, arguments)
        {
            UseShellExecute = true
        };

        using Process _ = Process.Start(psi);
    }
}
