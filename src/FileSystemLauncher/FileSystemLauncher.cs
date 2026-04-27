using FileSystemLauncher.Polyfill;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace FileSystemLauncher;

public static class FileSystemLauncher
{
    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static bool IsMacOS => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    public static void OpenFolder(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path must not be null or whitespace.", nameof(path));

        if (IsWindows)
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
        else if (IsMacOS)
        {
            ProcessStart("open", $"\"{path}\"");
        }
        else
        {
            ProcessStart("xdg-open", $"\"{path}\"");
        }
    }

    public static void OpenFolderAndSelectItem(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path must not be null or whitespace.", nameof(path));

        if (IsWindows)
        {
            ProcessStart("explorer.exe", $"/select,\"{path}\"");
        }
        else
        {
            throw new NotSupportedException("Select item is only supported on Windows.");
        }
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
