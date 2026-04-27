using System.Diagnostics;
using System.FileSystem.Polyfill;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace System.FileSystem;

/// <summary>
/// Provides methods to open files, folders, and URIs with the platform's default handlers.
/// </summary>
/// <remarks>
/// This class abstracts OS differences for launching files and folders:
/// on Windows it may use Explorer or ShellExecute; on macOS it uses <c>open</c>; on Linux it uses <c>xdg-open</c>.
/// </remarks>
public static class FileSystemLauncher
{
    /// <summary>
    /// Opens the specified file system path using the default application or file explorer.
    /// </summary>
    /// <param name="path">A file or folder path to open. Must not be <c>null</c> or whitespace.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is <c>null</c> or whitespace.</exception>
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

    /// <summary>
    /// Asynchronously opens the specified path using the default application or file explorer.
    /// </summary>
    /// <param name="path">A file or folder path to open. Must not be <c>null</c> or whitespace.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is <c>null</c> or whitespace.</exception>
    public static async Task OpenAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path must not be null or whitespace.", nameof(path));

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            string fullPath = path;
            if (!Path.IsPathRooted(fullPath)) fullPath = Path.GetFullPath(fullPath);

            bool launched;
            try
            {
                launched = await Launcher.LaunchUriAsync(new Uri(fullPath)).ConfigureAwait(false);
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
            await Task.CompletedTask;
        }
        else
        {
            ProcessStart("xdg-open", $"\"{path}\"");
            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// Opens the specified folder. This is an alias for <see cref="Open(string)"/>.
    /// </summary>
    /// <param name="path">Folder path to open.</param>
    public static void OpenFolder(string path)
    {
        Open(path);
    }

    /// <summary>
    /// Asynchronously opens the specified folder. This is an alias for <see cref="OpenAsync(string)"/>.
    /// </summary>
    /// <param name="path">Folder path to open.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    public static async Task OpenFolderAsync(string path)
    {
        await OpenAsync(path).ConfigureAwait(false);
    }

    /// <summary>
    /// Opens the folder that contains the specified path and selects the item in the file explorer.
    /// </summary>
    /// <param name="path">Path to the item to select. Must not be <c>null</c> or whitespace.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is <c>null</c> or whitespace.</exception>
    /// <exception cref="NotSupportedException">Thrown on non-Windows platforms because selection is only supported on Windows.</exception>
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

    /// <summary>
    /// Asynchronously opens the folder that contains the specified path and selects the item (Windows only).
    /// </summary>
    /// <param name="path">Path to the item to select. Must not be <c>null</c> or whitespace.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="path"/> is <c>null</c> or whitespace.</exception>
    /// <exception cref="NotSupportedException">Thrown on non-Windows platforms because selection is only supported on Windows.</exception>
    public static async Task OpenFolderAndSelectItemAsync(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Path must not be null or whitespace.", nameof(path));

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            ProcessStart("explorer.exe", $"/select,\"{path}\"");
            await Task.CompletedTask;
        }
        else
        {
            throw new NotSupportedException("Select item is only supported on Windows.");
        }
    }

    /// <summary>
    /// Starts a process using the shell with the provided file name and arguments.
    /// </summary>
    /// <param name="fileName">The process executable or shell verb to run (for example, <c>explorer.exe</c>).</param>
    /// <param name="arguments">Arguments to pass to the process.</param>
    private static void ProcessStart(string fileName, string arguments)
    {
        ProcessStartInfo psi = new(fileName, arguments)
        {
            UseShellExecute = true
        };

        using Process _ = Process.Start(psi);
    }
}
