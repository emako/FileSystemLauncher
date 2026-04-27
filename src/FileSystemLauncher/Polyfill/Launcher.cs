using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FileSystemLauncher.Polyfill
{
    public static class Launcher
    {
        private const int SW_SHOWNORMAL = 1;

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr ShellExecuteW(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);

        public static Task<bool> LaunchUriAsync(Uri uri)
        {
            return Task.FromResult(LaunchUri(uri));
        }

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

            IntPtr result = ShellExecuteW(IntPtr.Zero, "open", target, null, null, SW_SHOWNORMAL);
            long hinst = result.ToInt64();
            return hinst > 32;
        }
    }
}
