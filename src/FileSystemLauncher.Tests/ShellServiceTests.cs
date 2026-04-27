using System;
using Xunit;

namespace FileSystemLauncher.Tests;

public class ShellServiceTests
{
    private class FakeProcessRunner : IProcessRunner
    {
        public string FileName { get; private set; }

        public string Arguments { get; private set; }

        public int CallCount { get; private set; }

        public void Start(string fileName, string arguments)
        {
            FileName = fileName;
            Arguments = arguments;
            CallCount++;
        }
    }

    private class FakePlatformInfo : IPlatformInfo
    {
        public bool IsWindows { get; set; }
        public bool IsMacOS { get; set; }
        public bool IsLinux { get; set; }
    }

    [Fact]
    public void OpenFolder_OnWindows_StartsExplorerWithQuotedPath()
    {
        var runner = new FakeProcessRunner();
        var platform = new FakePlatformInfo { IsWindows = true };
        var service = new ShellService(runner, platform);

        service.OpenFolder(@"C:\Temp\My Folder");

        Assert.Equal("explorer.exe", runner.FileName);
        Assert.Equal("\"C:\\Temp\\My Folder\"", runner.Arguments);
        Assert.Equal(1, runner.CallCount);
    }

    [Fact]
    public void OpenFolderAndSelectItem_OnWindows_UsesSelectArg()
    {
        var runner = new FakeProcessRunner();
        var platform = new FakePlatformInfo { IsWindows = true };
        var service = new ShellService(runner, platform);

        service.OpenFolderAndSelectItem(@"C:\Temp\file.txt");

        Assert.Equal("explorer.exe", runner.FileName);
        Assert.Equal("/select,\"C:\\Temp\\file.txt\"", runner.Arguments);
        Assert.Equal(1, runner.CallCount);
    }

    [Fact]
    public void OpenFolderAndSelectItem_OnNonWindows_ThrowsNotSupported()
    {
        var runner = new FakeProcessRunner();
        var platform = new FakePlatformInfo { IsWindows = false, IsMacOS = true };
        var service = new ShellService(runner, platform);

        Assert.Throws<NotSupportedException>(() => service.OpenFolderAndSelectItem("/tmp/file.txt"));
    }
}
