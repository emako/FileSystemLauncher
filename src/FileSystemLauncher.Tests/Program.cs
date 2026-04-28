namespace System.FileSystem.Tests;

internal static class Program
{
    private static void Main()
    {
        FileSystemLauncher.OpenFolder(@"C:\Program Files");
        FileSystemLauncher.OpenFolderAndSelectItem(@"C:\Program Files\dotnet\dotnet.exe");
    }
}
