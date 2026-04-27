using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace System.FileSystem.Polyfill;

/// <summary>
/// Internal helper that launches URIs on Windows using the native <c>ShellExecuteW</c> API.
/// </summary>
internal static class Launcher
{
    /// <summary>
    /// Show-window command value for <c>ShellExecuteW</c> indicating a normal window.
    /// </summary>
    private const int SW_SHOWNORMAL = 1;

    /// <summary>
    /// Calls the Win32 <c>ShellExecuteW</c> function to perform the specified operation on a file or URI.
    /// </summary>
    /// <param name="hwnd">Window handle; use <c>IntPtr.Zero</c> when no owner window is provided.</param>
    /// <param name="lpOperation">Operation to perform (for example, <c>"open"</c>).</param>
    /// <param name="lpFile">File or URI to operate on.</param>
    /// <param name="lpParameters">Optional parameters for the operation.</param>
    /// <param name="lpDirectory">Working directory for the operation.</param>
    /// <param name="nShowCmd">Show-window command (for example, <c>SW_SHOWNORMAL</c>).</param>
    /// <returns>An integer result; values greater than 32 indicate success.</returns>
    [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern nint ShellExecuteW(nint hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

    /// <summary>
    /// Asynchronously attempts to launch the given <see cref="Uri"/> using the system shell.
    /// </summary>
    /// <param name="uri">The URI to launch.</param>
    /// <returns>A task that resolves to <c>true</c> if the launch succeeded; otherwise <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="uri"/> is <c>null</c>.</exception>
    public static Task<bool> LaunchUriAsync(Uri uri)
    {
        return Task.FromResult(LaunchUri(uri));
    }

    /// <summary>
    /// Attempts to launch the given <see cref="Uri"/> using the Windows shell.
    /// </summary>
    /// <param name="uri">The URI to launch. File URIs will use the local path when available.</param>
    /// <returns><c>true</c> if the shell reported success; otherwise <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="uri"/> is <c>null</c>.</exception>
    public static bool LaunchUri(Uri uri)
    {
        if (uri == null) throw new ArgumentNullException(nameof(uri));

        string target;

        if (uri.IsAbsoluteUri && uri.Scheme == Uri.UriSchemeFile)
        {
            target = uri.LocalPath;
            if (string.IsNullOrEmpty(target))
            {
                target = uri.OriginalString;
            }
        }
        else
        {
            target = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString();
        }

        nint result = ShellExecuteW(IntPtr.Zero, "open", target, null!, null!, SW_SHOWNORMAL);
        return result > 32;
    }
}
