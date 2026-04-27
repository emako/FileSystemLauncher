using System.Runtime.InteropServices;

namespace ShellApi;

public class PlatformInfo : IPlatformInfo
{
    public bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public bool IsMacOS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    public bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
}
