using System.Threading.Tasks;
using Microsoft.SqlServer.Management.RegisteredServers;
using RegisterSQLServers.SSMS.Model;

namespace RegisterSQLServers.SSMS.Interfaces.Services
{
    public interface ISSMSService
    {
        Task<ServerGroup> CreateOrGetRootServerGroup(string rootServerGroupName);

        Task<ServerGroup> CreateOrGetServerGroup(ServerGroup parent, string serverGroupName);
        
        Task<RegisteredServer> RegisterDatabaseInstance(ServerGroup environmentTierServerGroup, DatabaseInstance databaseInstance);
    }
}
