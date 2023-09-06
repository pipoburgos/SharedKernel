namespace SharedKernel.Integration.Tests.System.Threading.FileSystem;

public class FileSystemMutexTests : CommonMutexTests<FileSystemApp>
{
    public FileSystemMutexTests(FileSystemApp app1Mutex, FileSystemApp app2Mutex) : base(app1Mutex, app2Mutex)
    {
    }
}
