namespace SharedKernel.Application.ActiveDirectory
{
    public interface IActiveDirectoryService
    {
        bool IsConfigured { get; }

        bool Exists(string user, string password);
    }
}
