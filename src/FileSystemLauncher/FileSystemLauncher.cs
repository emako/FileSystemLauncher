using FileSystemLauncher.Polyfill;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FileSystemLauncher;

public static class FileSystemLauncher
{
    public static void Open(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path must not be null or whitespace.", nameof(path));

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
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
                ProcessStart("explorer.exe", $"\"{fullPath}\"");
            }
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            ProcessStart("open", $"\"{path}\"");
        }
        else
        {
            ProcessStart("xdg-open", $"\"{path}\"");
        }
    }

    public static async Task OpenAsync(string path)
    {
        // TODO
    }

    public static void OpenFolder(string path)
    {
        Open(path);
    }

    public static async Task OpenFolderAsync(string path)
    {
        // TODO
    }

    public static void OpenFolderAndSelectItem(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path must not be null or whitespace.", nameof(path));

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            ProcessStart("explorer.exe", $"/select,\"{path}\"");
        }
        else
        {
            throw new NotSupportedException("Select item is only supported on Windows.");
        }
    }

    public static async Task OpenFolderAndSelectItemAsync(string path)
    {
        // TODO
    }

    private static void ProcessStart(string fileName, string arguments)
    {
        ProcessStartInfo psi = new(fileName, arguments)
        {
            UseShellExecute = true
        };

        using Process _ = Process.Start(psi);
    }
}
