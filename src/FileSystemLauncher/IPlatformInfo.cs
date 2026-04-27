namespace FileSystemLauncher;

public interface IPlatformInfo
{
    public bool IsWindows { get; }

    public bool IsMacOS { get; }

    public bool IsLinux { get; }
}
