namespace SharedKernel.Application.ActiveDirectory;

/// <summary>
/// LDAP manager
/// </summary>
public interface IActiveDirectoryService
{
    /// <summary>
    /// Checks if ActiveDirectorySettings object is on appsettings.json
    /// </summary>
    bool IsConfigured { get; }

    /// <summary>
    /// Check that a user exists in the domain
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    bool Exists(string user, string password);
}