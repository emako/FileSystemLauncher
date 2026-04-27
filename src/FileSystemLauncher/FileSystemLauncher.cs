using System;
using System.IO;
using FileSystemLauncher.Polyfill;

namespace FileSystemLauncher;

public class FileSystemLauncher : IShellService
{
    private readonly IProcessRunner _processRunner;
    private readonly IPlatformInfo _platformInfo;

    public FileSystemLauncher()
    {
        _processRunner = new SystemProcessRunner();
        _platformInfo = new PlatformInfo();
    }

    public void OpenFolder(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path must not be null or whitespace.", nameof(path));

        if (_platformInfo.IsWindows)
        {
            string fullPath = path;
            if (!Path.IsPathRooted(fullPath)) fullPath = Path.GetFullPath(fullPath);

            bool launched;

            try
            {
                launched = Launcher.LaunchUri(new Uri(fullPath));
            }
            catch
            {
                launched = false;
            }

            if (!launched)
            {
                _processRunner.Start("explorer.exe", $"\"{fullPath}\"");
            }
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
