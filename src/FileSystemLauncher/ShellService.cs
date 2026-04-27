using System;

namespace ShellApi;

public class ShellService : IShellService
{
    private readonly IProcessRunner _processRunner;
    private readonly IPlatformInfo _platformInfo;

    public ShellService(IProcessRunner processRunner, IPlatformInfo platformInfo)
    {
        _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
        _platformInfo = platformInfo ?? throw new ArgumentNullException(nameof(platformInfo));
    }

    public ShellService() : this(new SystemProcessRunner(), new PlatformInfo())
    {
    }

    public void OpenFolder(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path must not be null or whitespace.", nameof(path));

        if (_platformInfo.IsWindows)
        {
            _processRunner.Start("explorer.exe", $"\"{path}\"");
        }
        else if (_platformInfo.IsMacOS)
        {
            _processRunner.Start("open", $"\"{path}\"");
        }
        else
        {
            _processRunner.Start("xdg-open", $"\"{path}\"");
        }
    }

    public void OpenFolderAndSelectItem(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path must not be null or whitespace.", nameof(path));

        if (_platformInfo.IsWindows)
        {
            _processRunner.Start("explorer.exe", $"/select,\"{path}\"");
        }
        else
        {
            throw new NotSupportedException("Select item is only supported on Windows.");
        }
    }
}
