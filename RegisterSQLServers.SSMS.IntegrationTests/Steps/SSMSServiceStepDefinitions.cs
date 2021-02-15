using FluentAssertions;
using Microsoft.SqlServer.Management.RegisteredServers;
using RegisterSQLServers.SSMS.Interfaces.Services;
using RegisterSQLServers.SSMS.Services;
using TechTalk.SpecFlow;

namespace RegisterSQLServers.SSMS.IntegrationTests.Steps
{
    [Binding]
    public sealed class SSMSServiceStepDefinitions
    {

        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly SSMSContext _ssmsContext;
        
        private RegisteredServersStore _localStore;
        private ServerGroup _result;

        public SSMSServiceStepDefinitions(SSMSContext ssmsContext)
        {
            _ssmsContext = ssmsContext;
        }

        [Given(@"the Local RegisteredServers Store does not contain a root ServerGroup called ""(.*)""")]
        public void GivenTheLocalRegisteredServersStoreDoesNotContainARootServerGroupCalled(string rootServerGroupName)
        {
            _localStore = RegisteredServersStore.LocalFileStore;
            var root = _localStore.DatabaseEngineServerGroup.ServerGroups[rootServerGroupName];
            root?.Drop();

            _ssmsContext.service = new SSMSService();
        }

        [When(@"an attempt is made to get or create a root ServerGroup called ""(.*)""")]
        public void WhenAnAttemptIsMadeToGetOrCreateARootServerGroupCalled(string rootServerGroupName)
        {
            _result = _ssmsContext.service.CreateOrGetRootServerGroup(rootServerGroupName).Result;
        }

        [Then(@"a ServerGroup called ""(.*)"" is returned")]
        public void ThenAServerGroupCalledIsReturned(string rootServerGroupName)
        {
            _result.Name.Should().Be(rootServerGroupName);
        }
    }

    public class SSMSContext
    {
        public SSMSService service { get; set; }
    }
}
