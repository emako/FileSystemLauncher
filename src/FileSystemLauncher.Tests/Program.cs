namespace System.FileSystem.Tests;

internal static class Program
{
    private static int Main(string[] args)
    {
        Console.WriteLine("FileSystemLauncher.Tests is now a console application.");

        ShellServiceTests.OpenFolder_OnWindows_StartsExplorerWithQuotedPath();
        ShellServiceTests.OpenFolderAndSelectItem_OnWindows_UsesSelectArg();
        return 0;
    }
}

public static class ShellServiceTests
{
    public static void OpenFolder_OnWindows_StartsExplorerWithQuotedPath()
    {
        FileSystemLauncher.OpenFolder(@"C:\Program Files");
    }

    public static void OpenFolderAndSelectItem_OnWindows_UsesSelectArg()
    {
        FileSystemLauncher.OpenFolderAndSelectItem(@"C:\Program Files\dotnet\dotnet.exe");
    }
}
