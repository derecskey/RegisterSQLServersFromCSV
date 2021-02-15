Feature: SSMSService Local Server Group Root
![SSMS](https://yefrinpacheco.com/cdn/resume/sql-server-SSMS-icon.png)
Service for programatically registering SQL Server Instances to the SSMS Registered Servers Local Server Group

Link to this feature: [SSMSService](RegisterSQLServers.SSMS.IntegrationTests/Features/SSMSService.feature)
***Further read***: **[Learn more about how to generate Living Documentation](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Generating-Documentation.html)**

@mytag
Scenario: Create the Local RegisteredServersStore
	Given the Local RegisteredServers Store does not contain a root ServerGroup called "IntegrationTests"
	When an attempt is made to get or create a root ServerGroup called "IntegrationTests"
	Then a ServerGroup called "IntegrationTests" is returned

