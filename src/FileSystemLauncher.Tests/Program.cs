namespace FileSystemLauncher.Tests;

internal static class Program
{
    private static int Main(string[] args)
    {
        Console.WriteLine("FileSystemLauncher.Tests is now a console application.");

        ShellServiceTests.OpenFolder_OnWindows_StartsExplorerWithQuotedPath();
        //ShellServiceTests.OpenFolderAndSelectItem_OnWindows_UsesSelectArg();
        return 0;
    }
}

public static class ShellServiceTests
{
    public static void OpenFolder_OnWindows_StartsExplorerWithQuotedPath()
    {
        var service = new FileSystemLauncher();
        service.OpenFolder(@"C:\Program Files");
    }

    public static void OpenFolderAndSelectItem_OnWindows_UsesSelectArg()
    {
        var service = new FileSystemLauncher();
        service.OpenFolderAndSelectItem(@"D:\GitHub\FileSystemLauncher\Logo.png");
    }
}
