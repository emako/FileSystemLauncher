[![NuGet](https://img.shields.io/nuget/v/FileSystemLauncher.svg)](https://nuget.org/packages/FileSystemLauncher) [![Actions](https://github.com/emako/FileSystemLauncher/actions/workflows/library.nuget.yml/badge.svg)](https://github.com/emako/FileSystemLauncher/actions/workflows/library.nuget.yml) 

# FileSystemLauncher

Cross-platform helpers to open files and folders in the user's file manager (Explorer, Finder, or the desktop environment's file browser).

This library provides simple synchronous and asynchronous APIs for launching the operating system's default file manager for a path or for selecting a specific file in its containing folder. On Windows the implementation uses a small polyfill that calls the native ShellExecute API and falls back to `explorer.exe` when necessary.

## Features

- Open a path or folder in the system file manager (synchronous and async)
- Select a specific file in Explorer on Windows
- Cross-platform support: Windows, macOS, Linux

## API

Public helpers are available from the static `FileSystemLauncher` class in the `FileSystemLauncher` namespace:

- `FileSystemLauncher.Open(string path)` — Open a file or folder synchronously.
- `Task FileSystemLauncher.OpenAsync(string path)` — Open asynchronously (uses native launcher when available).
- `FileSystemLauncher.OpenFolder(string path)` — Alias for `Open`.
- `Task FileSystemLauncher.OpenFolderAsync(string path)` — Async alias for `OpenAsync`.
- `FileSystemLauncher.OpenFolderAndSelectItem(string path)` — On Windows opens Explorer and selects the given item; throws `NotSupportedException` on non-Windows.
- `Task FileSystemLauncher.OpenFolderAndSelectItemAsync(string path)` — Async variant.

The implementation prefers the native shell launch APIs and provides robust fallbacks so calling code doesn't need to special-case platforms.

## Usage

Example (synchronous):

```csharp
using System.FileSystem;

// Open a folder or file path synchronously
FileSystemLauncher.Open(@"C:\Temp\My Folder");

// Select a file in Explorer (Windows only)
FileSystemLauncher.OpenFolderAndSelectItem(@"C:\Temp\file.txt");
```

Example (async):

```csharp
using System.FileSystem;
using System.Threading.Tasks;

await FileSystemLauncher.OpenAsync(@"C:\Temp\My Folder");
await FileSystemLauncher.OpenFolderAndSelectItemAsync(@"C:\Temp\file.txt");
```

## Build and run

Requires the .NET SDK. The library targets `netstandard2.0`; tests are a simple console project targeting `net6.0`.

Build the solution:

```bash
dotnet build
```

Run the test console app (example):

```bash
dotnet run --project src/FileSystemLauncher.Tests
```

## License

See the `LICENSE` file in the repository root.

Contributions and PRs are welcome.
