using Microsoft.SqlServer.Management.RegisteredServers;
using RegisterSQLServers.SSMS.Interfaces.Services;
using RegisterSQLServers.SSMS.Model;
using System.Threading.Tasks;

namespace RegisterSQLServers.SSMS.Services
{
    public class SSMSService : ISSMSService
    {
        private RegisteredServersStore _localRegisteredServersStore;
        private ServerGroup _databaseEngineServerGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="SSMSService"/> class.
        /// </summary>
        public SSMSService()
        {
            _localRegisteredServersStore = RegisteredServersStore.LocalFileStore;
            _databaseEngineServerGroup = _localRegisteredServersStore.DatabaseEngineServerGroup;
        }

        /// <summary>
        /// Creates the root server group if not exists.
        /// </summary>
        /// <param name="rootServerGroupName">Name of the root server group.</param>
        public async Task<ServerGroup> CreateOrGetRootServerGroup(string rootServerGroupName)
        {
            return await CreateOrGetServerGroup(_databaseEngineServerGroup, rootServerGroupName);
        }

        /// <summary>
        /// Creates the or get server group.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="serverGroupName">Name of the server group.</param>
        /// <returns></returns>
        public async Task<ServerGroup> CreateOrGetServerGroup(ServerGroup parent, string serverGroupName)
        {
            if (!parent.ServerGroups.Contains(serverGroupName))
            {
                var serverGroup = new ServerGroup(parent, serverGroupName);

                serverGroup.Create();
            }

            return parent.ServerGroups[serverGroupName];
        }

        /// <summary>
        /// Registers the database instance.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="databaseInstance">The database instance.</param>
        /// <returns></returns>
        public async Task<RegisteredServer> RegisterDatabaseInstance(ServerGroup parent, DatabaseInstance databaseInstance)
        {
            if (!parent.RegisteredServers.Contains(databaseInstance.DescriptiveName))
            {
                var registeredServer = new RegisteredServer(parent, databaseInstance.DescriptiveName)
                {
                    UseCustomConnectionColor = databaseInstance.UseCustomConnectionColor,
                    CustomConnectionColorArgb = databaseInstance.CustomConnectionColorARGB,
                    Description = databaseInstance.Description,
                    AuthenticationType = 0,
                    CredentialPersistenceType = CredentialPersistenceType.PersistLoginName,
                    ServerName = databaseInstance.FullyQualifiedInstanceName,
                    OtherParams = "initial catalog=" + databaseInstance.Name
                };

                registeredServer.Create();
            }

            return parent.RegisteredServers[databaseInstance.Name];
        }
    }
}
