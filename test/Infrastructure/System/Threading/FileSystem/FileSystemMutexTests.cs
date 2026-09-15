namespace SharedKernel.Integration.Tests.System.Threading.FileSystem;

public class FileSystemMutexTests : CommonMutexTests<FileSystemApp>
{
    public FileSystemMutexTests(TwoAppFixture<FileSystemApp> fixture) : base(fixture)
    {
    }
}
