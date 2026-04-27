namespace FileSystemLauncher;

public interface IShellService
{
    public void OpenFolder(string path);

    public void OpenFolderAndSelectItem(string path);
}
