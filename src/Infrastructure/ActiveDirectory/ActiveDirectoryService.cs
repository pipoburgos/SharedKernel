using SharedKernel.Application.ActiveDirectory;
using System.DirectoryServices;
using System.Linq;

namespace SharedKernel.Infrastructure.ActiveDirectory
{

    internal class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly ActiveDirectorySettings _settings;

        public ActiveDirectoryService(ActiveDirectorySettings settings)
        {
            _settings = settings;
        }


        public bool IsConfigured => !string.IsNullOrWhiteSpace(_settings.Path);

#pragma warning disable CA1416 // Validate platform compatibility
        public bool Exists(string user, string password)
        {

            var directorySearcher = new DirectorySearcher(new DirectoryEntry("LDAP://" + _settings.Path, user, password))
            {
                Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + user + "))"
            };


            var userResult = directorySearcher.FindOne();

            if (userResult == null)
                return false;

            var commonNames = _settings.CommonNamesKey.Contains(",")
                ? _settings.CommonNamesKey.Split(',')
                : new[] { _settings.CommonNamesKey };

            var organizationalUnits = _settings.OrganizationalUnitsKey.Contains(",")
                ? _settings.OrganizationalUnitsKey.Split(',')
                : new[] { _settings.OrganizationalUnitsKey };

            return userResult.Path
                .Replace($"LDAP://{_settings.Path}/", string.Empty)
                .Split(',')
                .Any(p => commonNames.Contains(p) || organizationalUnits.Contains(p));
        }
#pragma warning restore CA1416 // Validate platform compatibility
    }
}
